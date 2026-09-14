# TSD.BulkExtensions — Kế hoạch thay thế EFCore.BulkExtensions

Ngày: 2026-09-11. Cập nhật 2026-09-14. Trạng thái: **P0 merged (PR #1), P1 merged (PR #2), P2 xong** trên branch `feature/postgresql-adapter`. Kế tiếp: P3 (edge case, README, benchmark PG) hoặc P4 pilot.

P2 đã dựng (PostgreSQL đầy đủ):
- `Core/Transactions/OperationUnit`: tách từ SqlServer executor — mở transaction riêng khi không có transaction bao ngoài, dùng chung cho 2 provider.
- `PostgreSql/PostgreSqlSqlBuilder`: `CREATE TEMPORARY TABLE` staging (+`__Index` PK), `COPY … FORMAT BINARY`, gán khoá trước khi insert (`nextval(pg_get_serial_sequence)` theo thứ tự `__Index`, hoặc default SQL như `gen_random_uuid()`), `UPDATE … FROM` (chỉ hàng có cột compare khác, `IS DISTINCT FROM`), `INSERT … SELECT … WHERE NOT EXISTS … ORDER BY __Index` (+`OVERRIDING SYSTEM VALUE`), `DELETE … USING`, `DELETE … WHERE NOT EXISTS` (sync), đọc lại bằng JOIN theo khoá (không dựa vào thứ tự RETURNING).
- `PostgreSql/PostgreSqlMergeExecutor`: staged path cho Update/Delete/Upsert/Sync/Insert+SetOutputIdentity, Read, Truncate.
- `PostgreSql/PostgreSqlBulkAdapter`: đường nhanh `NpgsqlBinaryImporter` cho Insert thuần; `IsIdentityColumn` (IdentityAlways/ByDefault/Serial).
- Test: refactor `InsertTests/MergeTests/GraphTests/BatchTests` thành base provider-agnostic trong `Tests.Shared` (`ProviderTestBase` + hook `SupportsRowVersion`), lớp mỏng ở 2 provider; thêm PG `SqlBuilderTests`. Tổng 468 (SQL Server 78/80/80, PostgreSQL 76/78/78 theo net6/8/10).
- Khác thiết kế 4.3: upsert là UPDATE + INSERT (không `ON CONFLICT`, không cần unique index trên `UpdateByProperties`), không atomic với writer đồng thời (ghi trong README). `TRUNCATE` không `RESTART IDENTITY`.
- Gate 2 (code-review high) trên P2: 10 finding đã fix — Sync xoá nhầm hàng vừa insert (DELETE chạy trước INSERT), Insert staged copy CLR default của cột server-default (staging chỉ chứa cột INSERT chọn), KeepIdentity không đẩy sequence (`setval` sau insert), enum native của Npgsql bị ép về int, keyless + SetOutputIdentity sinh SQL lỗi (báo lỗi rõ), `IS NOT DISTINCT FROM` chỉ khi list có NULL, `BatchSize=0` gọi progress mỗi hàng (`BulkConfig.GetNotifyAfter` dùng chung), bỏ `nullWhenDefault`, gộp helper command vào `ConnectionScope` + `EntityTableMap.IsNoOp`, sửa doc option; `GraphExecutor` dùng `OperationUnit`; temp table `ON COMMIT DROP` khi tự mở transaction; đọc lại thẳng từ staging cho Insert thuần. Test 494/494 (SQL Server 82/84/84, PG 80/82/82).

P1 đã dựng (SQL Server đầy đủ):
- `Core/Metadata`: `EntityTableMap`, `ColumnMap`, `PropertyAccessor` (đọc model EF thành bản đồ cột, include/exclude, khoá match, identity, default, rowversion, owned, discriminator, converter).
- `Core/Streaming/EntityDataReader`: `DbDataReader` stream + cột `__Index`.
- `Core/Transactions/ConnectionScope`: dùng lại connection/transaction của DbContext.
- `Core/Output/OutputWriteBack`: ghi giá trị server sinh về entity theo `__Index`, thống kê I/U/D.
- `Core/Graph`: `EntityGraphWalker` + `GraphExecutor` (IncludeGraph theo lượt, cha trước con, FK lan truyền, transaction bao ngoài). Provider-agnostic.
- `Core/Batch/QueryableBatchExtensions`: `BatchDelete/BatchUpdate` bọc `ExecuteDelete/ExecuteUpdate` (EF 8 dùng `SetPropertyCalls`, EF 10 dùng `UpdateSettersBuilder`); EF 6 ném `NotSupportedException`.
- `SqlServer`: `SqlServerBulkAdapter` (đường nhanh SqlBulkCopy), `SqlServerSqlBuilder` (T-SQL), `SqlServerMergeExecutor` (#temp + MERGE OUTPUT, Read, Truncate).
- `benchmarks/`: BenchmarkDotNet so với EFCore.BulkExtensions 6.5.6 và SaveChanges.
- Test: 325 (SQL Server 78/80/80 theo net6/8/10, PostgreSQL 29 × 3).
- Gate 2 (code-review high) trên P1: 10 finding đã fix (TPH/private setter qua MemberInfo của EF, proxy qua `FindRuntimeEntityType`, cấm IncludeGraph+Sync, no-op MERGE bằng biến, BulkRead tôn trọng include list, stats gộp qua graph, staged op tự mở transaction, owned type bảng riêng báo lỗi rõ, map cache độc lập delegate shadow) + cleanup (clustered `__Index`, gộp CREATE/DROP, batch net6 thành lỗi compile, pin upstream vào `benchmarks/Directory.Packages.props`).

Khác so với thiết kế ban đầu: bảng tạm luôn là `#temp` session-local (không cần transaction, `UseTempDB` thành no-op); ánh xạ Id theo `__Index` thay vì thứ tự MERGE.

P0 đã dựng: `TSD.BulkExtensions.slnx`, CPM 3 dòng EF, Core (BulkConfig Tier 1, DbContextBulkExtensions đủ 30 signature, BulkExecutor, BulkAdapterRegistry, IBulkAdapter), adapter SqlServer/PostgreSql stub, Tests.Shared (model Item/ItemHistory Guid-default/Order/OrderLine + SmokeTestsBase), 2 test project TestContainers. CI GitHub Actions tạm bỏ theo quyết định của Tiền bối (thêm lại khi token có scope `workflow`). Build 0 error/0 warning, 66/66 test pass (11 × 2 provider × 3 TFM) sau Gate 2.

## 1. Bối cảnh

- EFCore.BulkExtensions (borisdj) đổi sang dual license từ 22/01/2023 (v6.7.0+). Công ty vượt ngưỡng 1M USD doanh thu → phải mua commercial (1.000–4.800 USD/năm theo số dev).
- Fork MIT (videokojot) là drop-in về API nhưng: 1 maintainer, 23 open issue, 10 open PR, release cuối 09/12/2025, push cuối 17/03/2026. Không đủ tin cậy để phụ thuộc lâu dài.
- Quyết định: **viết lại thư viện nội bộ**, kiến trúc riêng, scope hẹp theo nhu cầu thực tế, harvest có chọn lọc từ code MIT (hợp pháp, giữ attribution).

## 2. Scope đã chốt

| Hạng mục | Giá trị |
|---|---|
| Provider | SQL Server, PostgreSQL |
| EF Core / .NET | 6 (net6.0), 8 (net8.0), 10 (net10.0) |
| Điểm tích hợp | `BulkRepository<TEntity,TPrimaryKey>` / `IBulkRepository` (ABP), consumer inject và gọi |
| Ràng buộc | Public API + signature giữ nguyên 100% so với hiện tại |
| Namespace | `TSD.BulkExtensions` |
| Đóng gói | 1 package, 3 TFM `net6.0;net8.0;net10.0` |
| Repo | `E:\ThaiSon\TSDBulkExtension`, git private |

Ghi chú: EF Core 6 / net6.0 đã EOL (11/2024). Vẫn hỗ trợ vì dự án đang chạy, nhưng đánh dấu là target sẽ bỏ khi các dự án đó lên .NET 8.

### 2.1 Kết quả scan TruePos (2026-09-11, `scan/TruePos/report.md`)

| Chỉ số | Giá trị |
|---|---|
| Solution | TruePos, 62 csproj, net6.0, ABP 7.4.0, EF Core 6.0.10 |
| Provider | SQL Server only |
| EFCore.BulkExtensions | **6.5.6** (08/2022, MIT, trước khi đổi license) |
| Class routing | `POSBulkRepository` (TSD.POS.EntityFrameworkCore) |
| Call site | BulkInsertAsync 317, BulkUpdateAsync 233, BulkDeleteAsync 98, BulkInsertOrUpdateAsync 42, IQueryable `BatchDeleteAsync` 11 |
| Không consumer nào gọi | BulkRead, BulkInsertOrUpdateOrDelete, Truncate (chỉ có trong wrapper) |
| Option dùng thật (scan v2, đã sửa đếm) | `SetOutputIdentity` 140 hit/69 file · `PropertiesToInclude` 82/36 · `WithHoldlock=false` 79/25 · `IncludeGraph` 62/36 · `BatchSize` 26/9 · `PreserveInsertOrder` 20/15 · `UseTempDB` 1/1 |
| Model feature (ngoài Migrations) | Rất đơn giản: không owned type, không value converter, không rowversion, không TPH/TPT. Có 1 global query filter (`IsDeleted`), 1 `HasDefaultValueSql("newsequentialid()")` trên PK Guid |
| Kiểu khoá | Đa số `Entity<long>` identity; nhóm Acen dùng `AuditedEntity<Guid>` |
| File `using EFCore.BulkExtensions` trực tiếp | 8 (+ POSBulkRepository) |
| Thư viện ZZZ khác | `Abp.EntityFrameworkCore.EFPlus` (Z.EntityFramework.Plus, MIT) dùng cho `repo.BatchDeleteAsync(predicate)` |

Tier 1 rút gọn theo bằng chứng TruePos: 7 option trên + `SetOutputIdentity` phải hoạt động với cả identity `long` **và** PK `Guid` có default `newsequentialid()` (đường "DefaultValueProperties chứa PK" trong TableInfo). `IncludeGraph` phải hỗ trợ cả `BulkUpdateAsync` (InventoryCountsManager) chứ không chỉ Insert/Upsert. Owned/converter/rowversion/TPH vẫn xây nhưng ưu tiên sau, chờ scan các solution EF 8/10.

Hệ quả cho thiết kế:
- `IncludeGraph` chuyển lên **Tier 1**. Đây là feature phức tạp nhất (duyệt graph, sắp thứ tự theo FK, gán FK sau khi insert cha với `SetOutputIdentity`).
- IQueryable `BatchDelete/BatchUpdate`: EF 8/10 → wrapper mỏng trên `ExecuteDelete/ExecuteUpdate`. EF 6 → không có API native; 11 call site của TruePos chuyển sang `repo.BatchDeleteAsync(predicate)` của ABP EFPlus (đã dùng 17 chỗ khác) thay vì tự viết parser SQL (~1.000 LOC trong fork).
- `BulkRead`, `BulkInsertOrUpdateOrDelete`, `Truncate`: giữ API, làm sau cùng, ưu tiên thấp.
- TruePos hiện dùng bản MIT (6.5.6) nên chưa vi phạm. Rủi ro phát sinh khi nâng EF 8/10 (phải lên 8.x/10.x dual license). Các solution EF 8/10 khác cần scan tiếp.

## 3. Public API (không đổi)

Namespace đề xuất: `TSD.BulkExtensions` (consumer đổi 1 dòng `using`). Phương án thay thế: giữ `EFCore.BulkExtensions` để zero-change. Cần confirm.

```csharp
// DbContext extensions — signature giống hệt bản đang dùng
void BulkInsert<T>(this DbContext ctx, IList<T> entities, BulkConfig? cfg = null, Action<decimal>? progress = null, Type? type = null)
void BulkInsert<T>(this DbContext ctx, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null)
Task BulkInsertAsync<T>(... , CancellationToken ct = default)
// Tương tự cho: BulkUpdate, BulkDelete, BulkInsertOrUpdate, BulkInsertOrUpdateOrDelete, BulkRead
void Truncate<T>(this DbContext ctx, Type? type = null)
Task TruncateAsync<T>(this DbContext ctx, Type? type = null, CancellationToken ct = default)
```

`BulkConfig` — Tier 1 (hỗ trợ đầy đủ):

| Property | Ghi chú |
|---|---|
| `PreserveInsertOrder` | default true |
| `SetOutputIdentity` | trả identity/DB-generated về entity |
| `BatchSize` | default 2000 |
| `NotifyAfter`, `BulkCopyTimeout`, `EnableStreaming` | |
| `UseTempDB`, `UniqueTableNameTempDb` | |
| `CustomDestinationTableName` | |
| `PropertiesToInclude`, `PropertiesToExclude` | |
| `PropertiesToIncludeOnUpdate`, `PropertiesToExcludeOnUpdate` | |
| `PropertiesToIncludeOnCompare`, `PropertiesToExcludeOnCompare` | |
| `UpdateByProperties` | upsert theo key tuỳ chọn |
| `EnableShadowProperties`, `ShadowPropertyValue` | |
| `IgnoreRowVersion` | |
| `WithHoldlock` | SQL Server only |
| `SqlBulkCopyOptions` | SQL Server only |
| `OnConflictUpdateWhereSql` | PostgreSQL only |
| `CalculateStats` → `StatsInfo` | |
| `IncludeGraph` | **bắt buộc** theo scan TruePos; insert/upsert cả graph navigation, gán FK sau khi có identity cha |

Batch ops trên `IQueryable` (`BatchDelete`, `BatchUpdate`): EF 8/10 → wrapper trên `ExecuteDelete/ExecuteUpdate`; EF 6 → ném `NotSupportedException`, consumer chuyển sang ABP EFPlus `BatchDeleteAsync(predicate)`.

Tier 2 (không hỗ trợ ở v1, ném `NotSupportedException` với message rõ, không âm thầm bỏ qua): `BulkSaveChanges`, spatial, HierarchyId, TPT/TPC, temporal table, JSON column, `OnSaveChangesSetFK`, `CustomSqlPostProcess`, `DataReader`.

Nếu inventory ở Phase 0 phát hiện consumer đang dùng option Tier 2 → chuyển lên Tier 1.

## 4. Kiến trúc

```
TSDBulkExtension/
├─ src/
│  ├─ TSD.BulkExtensions.Core/          # BulkConfig, DbContextBulkExtensions, EntityTableMap, IBulkAdapter, BulkException
│  ├─ TSD.BulkExtensions.SqlServer/     # SqlServerBulkAdapter (SqlBulkCopy + MERGE)
│  └─ TSD.BulkExtensions.PostgreSql/    # PostgreSqlBulkAdapter (COPY BINARY + ON CONFLICT)
├─ tests/
│  ├─ TSD.BulkExtensions.Tests.Shared/  # TestContext model, test case dùng chung
│  ├─ TSD.BulkExtensions.Tests.SqlServer/   # TestContainers mssql
│  └─ TSD.BulkExtensions.Tests.PostgreSql/  # TestContainers postgres
├─ benchmarks/TSD.BulkExtensions.Benchmarks/  # BenchmarkDotNet, so với lib cũ
├─ samples/BulkRepository/              # BulkRepository.cs + IBulkRepository.cs mẫu tích hợp ABP
├─ Directory.Build.props, Directory.Packages.props (CPM), global.json
├─ LICENSE (MIT), NOTICE (attribution borisdj 2017, Jindrich Cincibuch 2023)
└─ .github/workflows | .gitlab-ci.yml
```

Multi-target: **1 package, 3 TFM** `net6.0;net8.0;net10.0`, PackageReference theo điều kiện TFM (EF 6.0.x / 8.0.x / 10.0.x, Npgsql.EFCore 6/8/10, Microsoft.Data.SqlClient). Khác biệt API EF giữa các bản xử lý bằng `#if NET8_0_OR_GREATER`. Version package semver riêng (1.x), không gắn EF major.

### 4.1 Core

- `EntityTableMap`: build từ `context.Model.FindEntityType(type)`. Cache theo `(IModel, Type, config-hash)`. Gồm: schema/table, danh sách cột (tên cột, CLR type, provider type, `ValueConverter`, isKey, isIdentity/ValueGeneratedOnAdd, isComputed, isRowVersion, isShadow, default), owned type flatten (table splitting), TPH discriminator.
- `IBulkAdapter`: `Insert`, `Merge(BulkOperation)`, `Read`, `Truncate` — sync + async.
- `EntityDataReader<T>`: `IDataReader` streaming trên `IList<T>` (không DataTable → bộ nhớ ổn định với 1M row). Getter compile bằng expression tree, cache.
- Transaction: nếu `Database.CurrentTransaction != null` (ABP UnitOfWork đã mở) → dùng lại, không commit. Nếu chưa có → tạo, commit khi xong, rollback khi lỗi.
- `BulkException`: bọc lỗi DB kèm SQL đã sinh + tên bảng + operation.
- Diagnostics: `ActivitySource("TSD.BulkExtensions")`, log qua `ILogger` của EF.

### 4.2 SQL Server adapter

- Insert: `SqlBulkCopy` (DataReader streaming, `BatchSize`, `NotifyAfter` → progress).
- Upsert/Update/Delete: BulkCopy vào `#Tmp_<Table>_<guid>` (hoặc global `##` khi `UseTempDB`) → `MERGE ... WITH (HOLDLOCK)` → `OUTPUT` để map identity; cột `[__Index]` giữ thứ tự khi `PreserveInsertOrder`.
- Delete: `MERGE ... WHEN MATCHED THEN DELETE` hoặc `DELETE t FROM t JOIN #tmp`.
- Read: BulkCopy key vào temp → `SELECT ... FROM t JOIN #tmp ON keys` → gán ngược vào entity.
- Truncate: `TRUNCATE TABLE`.

### 4.3 PostgreSQL adapter

- Insert: `NpgsqlBinaryImporter` (`COPY ... FROM STDIN (FORMAT BINARY)`).
- Staged: COPY vào `CREATE TEMPORARY TABLE` (+`__Index`), gán khoá cho hàng mới ngay trong staging (sequence theo `__Index` / default SQL) khi `SetOutputIdentity`.
- Upsert: `UPDATE t SET ... FROM stg WHERE keys AND (compare) IS DISTINCT FROM ... [AND OnConflictUpdateWhereSql]` rồi `INSERT ... SELECT ... FROM stg WHERE NOT EXISTS (...) ORDER BY __Index`. Không dùng `ON CONFLICT` nên không cần unique index trên `UpdateByProperties`; đổi lại không atomic với writer đồng thời (đã ghi README).
- Update thuần: `UPDATE ... FROM stg` (tránh issue #171 của fork).
- Delete: `DELETE FROM t USING stg WHERE keys`; Sync: thêm `DELETE ... WHERE NOT EXISTS`.
- Read / ghi giá trị server sinh về: `SELECT stg.__Index, t.cols FROM stg JOIN t ON keys`.
- Truncate: `TRUNCATE TABLE` (không RESTART IDENTITY).

## 5. Harvest từ fork MIT (hợp pháp, có attribution)

| Lấy gì | Từ đâu | Cách dùng |
|---|---|---|
| Logic ánh xạ metadata (converter, shadow, owned, identity, computed, rowversion) | `TableInfo.cs` | Đọc → viết lại vào `EntityTableMap` theo kiến trúc mới |
| MERGE + OUTPUT builder, xử lý identity mapping | `SqlServerAdapter.cs`, `SqlQueryBuilder.cs` | Tham khảo, viết lại |
| COPY BINARY + ON CONFLICT | `PostgreSqlAdapter.cs` | Tham khảo, viết lại |
| Test model + test case | `EFCore.BulkExtensions.Tests/` | Port trực tiếp làm acceptance spec |

**Tuyệt đối không** copy từ repo gốc (borisdj) commit sau 22/01/2023.

## 6. Test strategy

- TestContainers: `mcr.microsoft.com/mssql/server:2022-latest`, `postgres:16`. Cần Docker trên máy dev và CI runner.
- Matrix: 3 TFM × 2 provider.
- Nhóm test: CRUD cơ bản; identity output + PreserveInsertOrder; composite key; UpdateByProperties; shadow property; value converter (enum→string, DateOnly, Guid→string); owned type; rowversion; TPH; schema-qualified table; UseTempDB; progress callback; cancellation; transaction có sẵn (giả lập ABP UoW); batch 100K row; Tier 2 → NotSupportedException.
- Benchmark: BenchmarkDotNet, so với EFCore.BulkExtensions hiện tại trên 10K / 100K / 1M row. Mục tiêu: không chậm hơn 10%.

## 7. Phase & timeline (1 dev; 2 dev thì P1/P2 song song)

| Phase | Nội dung | Thời gian |
|---|---|---|
| P0 | `git init`, scaffold solution, CPM, CI skeleton, TestContainers chạy được. Inventory option `BulkConfig` đang dùng ở consumer (cần đường dẫn solution) | 2 ngày |
| P1 | Core + SQL Server adapter trên EF 8 → bật net6.0/net10.0 | 2 tuần |
| P2 | PostgreSQL adapter | 1.5 tuần |
| P3 | Port test edge case từ fork, benchmark, README | 1 tuần |
| P4 | Pilot: 1 dự án SQL Server + 1 dự án PostgreSQL. So sánh SQL sinh ra qua interceptor, chạy integration test dự án | 3 ngày |
| P5 | Rollout từng dự án (đổi PackageReference + `using`), `dotnet list package --include-transitive` xác nhận sạch package gốc, rule CI chặn `EFCore.BulkExtensions` | 0.5 ngày/dự án |

Tổng: ~6–7 tuần (1 dev), ~4.5 tuần (2 dev).

## 8. Quy trình mỗi bước (theo WORKFLOW.md)

- Branch `feature/<tên>` cho mỗi phase, không commit thẳng master.
- Gate 1: build 0 error → test pass → `caveman:caveman-review` → fix → báo cáo.
- Gate 2 trước commit/MR: `/code-review high` → fix → build + test lại → MR có section Review.

## 9. Cần confirm

1. Namespace: `TSD.BulkExtensions` (đổi `using`) hay giữ `EFCore.BulkExtensions` (zero-change)?
2. Danh sách Tier 1 / Tier 2 ở mục 3 ổn chưa? Hoặc cho đường dẫn 1–2 solution để inventory chính xác.
3. 1 package 3 TFM (đề xuất) hay 3 package theo EF major?
4. Repo đặt tại `E:\ThaiSon\TSDBulkExtension` (git init mới)? NuGet feed nội bộ nào?
5. Đồng ý bắt đầu P0?
