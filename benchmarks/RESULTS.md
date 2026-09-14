# Benchmark results

2026-09-14, `TSD.BulkExtensions.Benchmarks` (net6.0, in-process, RunStrategy=Monitoring, 3 iterations) against
EFCore.BulkExtensions 6.5.6 (last MIT release) and plain `SaveChanges`, SQL Server 2022 in Docker on the same machine.
Intel i7-6820HQ, .NET SDK 10.0.401 (host runtime 9.0.17). Wall-clock numbers of a laptop: read the ratios, not the absolutes.

| Operation | N | TSD | EFCore.BulkExtensions 6.5.6 | SaveChanges | TSD vs upstream | Allocated TSD / upstream |
|---|---:|---:|---:|---:|---:|---:|
| Insert | 10 000 | 168 ms | 238 ms | 1 955 ms | 0.70× | 2.0 MB / 13.8 MB |
| Insert | 50 000 | 750 ms | 952 ms | skipped | 0.79× | 9.7 MB / 66.1 MB |
| Insert + generated keys | 10 000 | 252 ms | 519 ms | | 0.49× | 4.8 MB / 22.8 MB |
| Insert + generated keys | 50 000 | 782 ms | 1 718 ms | | 0.46× | 22.9 MB / 110.0 MB |
| Update | 10 000 | 414 ms | 541 ms | | 0.77× | 2.5 MB / 14.5 MB |
| Update | 50 000 | 1 596 ms | 2 069 ms | | 0.77× | 12.1 MB / 69.2 MB |

Why the gap:

- Plain insert streams through a `DbDataReader`; upstream builds a `DataTable` first (≈7× the allocations).
- Insert with generated keys reads them back by row index from one OUTPUT table; upstream copies the identity column
  into the staging table, rewrites it with negative placeholders and re-reads the output ordered by id.
- Update stages into a session temp table with a clustered `__Index` and bulk-copies with an order hint, so the ordered
  MERGE source needs no sort.

Reproduce:

```bash
cd benchmarks/TSD.BulkExtensions.Benchmarks
dotnet run -c Release --framework net6.0 -- --filter "*BulkBenchmarks*"
```

Set `TSD_BENCH_SQLSERVER` to a connection string to use an existing server instead of a container.
