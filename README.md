# TSD.BulkExtensions

Bulk Insert / Update / Delete / Upsert / Read / Truncate for EF Core on **SQL Server** and **PostgreSQL**.
In-house, MIT-licensed replacement for EFCore.BulkExtensions with an identical public API.

| Package | Purpose |
|---|---|
| `TSD.BulkExtensions.Core` | `BulkConfig`, `DbContext` extension methods, EF metadata mapping, adapter contract |
| `TSD.BulkExtensions.SqlServer` | `SqlBulkCopy` + `MERGE` adapter |
| `TSD.BulkExtensions.PostgreSql` | `COPY BINARY` + `INSERT … ON CONFLICT` adapter |

One package per provider, each shipping `net6.0`, `net8.0` and `net10.0` builds bound to EF Core 6 / 8 / 10.

## Migrating from EFCore.BulkExtensions

1. Replace the `EFCore.BulkExtensions` package reference with `TSD.BulkExtensions.SqlServer` and/or `TSD.BulkExtensions.PostgreSql`.
2. Replace `using EFCore.BulkExtensions;` with `using TSD.BulkExtensions;`.
3. Rebuild. Method signatures and `BulkConfig` option names are unchanged.

Options that are not supported are not present on `BulkConfig`, so unsupported usage fails at compile time rather than silently at runtime. The library never mutates the `BulkConfig` you pass in; only `StatsInfo` is written back.

## Repository layout

```
src/        library projects
tests/      xunit + Testcontainers (Docker required)
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
