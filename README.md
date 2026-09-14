# TSD.BulkExtensions

Bulk Insert / Update / Delete / Upsert / Read / Truncate for EF Core on **SQL Server** and **PostgreSQL**.
In-house, MIT-licensed replacement for EFCore.BulkExtensions with an identical public API.

| Package | Purpose |
|---|---|
| `TSD.BulkExtensions.Core` | `BulkConfig`, `DbContext` extension methods, EF metadata mapping, adapter contract |
| `TSD.BulkExtensions.SqlServer` | `SqlBulkCopy` + `MERGE` adapter |
| `TSD.BulkExtensions.PostgreSql` | `COPY BINARY` + `UPDATE … FROM` / `INSERT … SELECT` / `DELETE … USING` adapter |

One package per provider, each shipping `net6.0`, `net8.0` and `net10.0` builds bound to EF Core 6 / 8 / 10.

## Migrating from EFCore.BulkExtensions

1. Replace the `EFCore.BulkExtensions` package reference with `TSD.BulkExtensions.SqlServer` and/or `TSD.BulkExtensions.PostgreSql`.
2. Replace `using EFCore.BulkExtensions;` with `using TSD.BulkExtensions;`.
3. Rebuild. Method signatures and `BulkConfig` option names are unchanged.

Options that are not supported are not present on `BulkConfig`, so unsupported usage fails at compile time rather than silently at runtime. The library never mutates the `BulkConfig` you pass in; only `StatsInfo` is written back.

Behavioural notes:

- Staging always uses a session-local temp table held on the context's open connection, so no transaction is required and `UseTempDB` is accepted but has no effect.
- Generated values are mapped back by source row index, not by insertion order; `PreserveInsertOrder` only asks SQL Server to assign identities in list order.
- `BulkInsertOrUpdateOrDelete` deletes every row of the table that is not in the list. Upstream's `SynchronizeFilter` is not supported, and the operation cannot be combined with `IncludeGraph`.
- Without an ambient transaction, staged operations open their own so the statement and the write-back of generated values succeed or fail together.
- On EF Core 6 the `IQueryable` batch methods (`BatchDelete`, `BatchUpdate`) are compile errors pointing to the ABP EFPlus repository methods; on EF Core 8/10 they wrap `ExecuteDelete` / `ExecuteUpdate`.
- Owned types mapped to their own table are not supported (a clear exception with `IncludeGraph`, columns ignored otherwise).
- PostgreSQL: an upsert runs as `UPDATE … FROM` followed by `INSERT … WHERE NOT EXISTS` inside one transaction, not as `INSERT … ON CONFLICT`. A concurrent writer inserting the same key between the two statements surfaces as a unique-violation error. Generated keys for new rows are taken from the table's sequence in list order before the insert.
- PostgreSQL: a source row matching several table rows updates all of them; the error SQL Server raises for that case only appears when `SetOutputIdentity` is on (the read-back detects the duplicate).
- `SqlBulkCopyOptions.KeepIdentity` is honoured on both providers. On PostgreSQL the identity sequence is moved past the inserted keys afterwards (`setval`), matching SQL Server's automatic reseed; `GENERATED ALWAYS` identities are written with `OVERRIDING SYSTEM VALUE`.
- PostgreSQL: `SetOutputIdentity` reads generated values back by joining on the key, so the entity needs a primary key (keyless entities are rejected with a clear error) and a `CustomDestinationTableName` must own the identity sequence.
- PostgreSQL: nullable match columns compare with `IS NOT DISTINCT FROM` only when the list actually contains a NULL for them; otherwise plain `=` keeps hash joins and indexes usable.
- PostgreSQL: enums mapped natively (`HasPostgresEnum`) are written as the enum; enums with a value converter or without a native mapping go through the converter / as their underlying number, as on SQL Server.

## Status

| Provider | Insert | Insert + generated keys | Update | Delete | Upsert | Sync | Read | Truncate | IncludeGraph | Batch (IQueryable) |
|---|---|---|---|---|---|---|---|---|---|---|
| SQL Server | done | done | done | done | done | done | done | done | done | EF 8/10 |
| PostgreSQL | done | done | done | done | done | done | done | done | done | EF 8/10 |

How it works, in one paragraph: a plain insert streams the list into the table with `SqlBulkCopy` through a forward-only
`DbDataReader` (no `DataTable`). Every other operation streams into a session-local temp table together with each row's
position, runs a single `MERGE` (or `SELECT` for reads) against it, and reads generated values back through an `OUTPUT`
table keyed by that position, so identities, server defaults and rowversions land on the right entity regardless of
insertion order. On PostgreSQL the same staging table is filled with `COPY … FORMAT BINARY`; new rows get their keys
assigned in the staging table first (sequence or default expression), the set statements run, and generated values are
read back by joining on the key, so nothing depends on `RETURNING` order. `IncludeGraph` walks the navigations, saves one
entity type per pass parents-first inside one transaction, and copies generated keys into the children's foreign keys
between passes. Details in `docs/bulk-extension-internals.html`.

## Repository layout

```
src/        library projects
tests/      xunit + Testcontainers (Docker required)
benchmarks/ BenchmarkDotNet: TSD vs EFCore.BulkExtensions 6.5.6 (last MIT release) vs SaveChanges
samples/    ABP BulkRepository integration sample
docs/       design notes (see PLAN.md and docs/bulk-extension-internals.html)
tools/      offline usage scanner for consumer solutions
```

## Build and test

```bash
dotnet build
dotnet test
```

Tests start SQL Server 2022 and PostgreSQL 16 containers through Testcontainers. Docker Desktop (Linux containers) must be running.

## License

MIT. See `LICENSE` and `NOTICE` for attribution of the MIT-licensed upstream work this project derives from.
