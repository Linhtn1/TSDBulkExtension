# ABP integration sample

`IBulkRepository.cs` and `BulkRepository.cs` are the routing base used by every consuming project
(ASP.NET Boilerplate `IDbContextProvider<TDbContext>`). They are kept here verbatim as the compatibility target:
every method they call must keep compiling against `TSD.BulkExtensions` with only the `using` line changed.

Migration for a consumer project:

```diff
-using EFCore.BulkExtensions;
+using TSD.BulkExtensions;
```

and in the `.csproj`:

```diff
-<PackageReference Include="EFCore.BulkExtensions" Version="6.5.6" />
+<PackageReference Include="TSD.BulkExtensions.SqlServer" Version="0.1.0-preview" />
```

These files are not compiled as part of this repository (they depend on the consumer's ABP DbContext).
