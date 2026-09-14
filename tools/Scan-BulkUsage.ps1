<#
.SYNOPSIS
    Offline scanner: inventories EFCore.BulkExtensions usage across one or more solution folders.
    No internet, no external modules. Works on Windows PowerShell 5.1 and PowerShell 7+.

.USAGE
    powershell -ExecutionPolicy Bypass -File .\Scan-BulkUsage.ps1 -Root "D:\src\ProjectA;D:\src\ProjectB" -Out "D:\bulk-scan"

    Multiple roots: separate with ';' (or ','). A single root works as-is.

    Then copy the whole "D:\bulk-scan" folder back (report.md + report.json).

.WHAT IT COLLECTS
    - Per .csproj: TargetFramework(s), EFCore.BulkExtensions version, EF Core / provider package versions
    - DB providers used (UseSqlServer / UseNpgsql / UseMySql ...)
    - Every call site of Bulk* / Batch* / Truncate methods (file:line + trimmed code line)
    - Every BulkConfig option name referenced (counted, with file:line samples)
    - EF model features that affect bulk mapping (OwnsOne, HasConversion, shadow props, RowVersion, TPT/TPC, computed columns, JSON, spatial...)

.PRIVACY
    - Skips bin/, obj/, .git/, node_modules/, packages/, Migrations/, appsettings*.json, *.config, *.pfx, *.snk
    - Only emits source-code lines that match a pattern. Review report.md before copying if needed.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string[]] $Root,

    [string] $Out = (Join-Path (Get-Location) "bulk-scan"),

    # Max sample lines per pattern kept in the report (all hits still counted)
    [int] $MaxSamples = 40
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version 2

# When invoked via `powershell -File`, a comma-separated -Root arrives as ONE string. Normalize.
$Root = @($Root | ForEach-Object { $_ -split '[;,]' } | ForEach-Object { $_.Trim().Trim('"').Trim("'") } | Where-Object { $_ })

# ---------- pattern catalog ----------
$BulkMethods = @(
    "BulkInsert", "BulkInsertAsync", "BulkUpdate", "BulkUpdateAsync", "BulkDelete", "BulkDeleteAsync",
    "BulkInsertOrUpdate", "BulkInsertOrUpdateAsync", "BulkInsertOrUpdateOrDelete", "BulkInsertOrUpdateOrDeleteAsync",
    "BulkRead", "BulkReadAsync", "Truncate", "TruncateAsync",
    "BatchUpdate", "BatchUpdateAsync", "BatchDelete", "BatchDeleteAsync",
    "BulkSaveChanges", "BulkSaveChangesAsync"
)

$BulkConfigOptions = @(
    "ApplySubqueryLimit", "AutoExcludeTimestamp", "BatchSize", "BulkCopyTimeout", "CalculateStats", "ConflictOption",
    "CustomDestinationTableName", "CustomSourceTableName", "CustomSourceDestinationMappingColumns", "CustomSqlPostProcess",
    "DataReader", "DoNotUpdateIfTimeStampChanged", "EnableShadowProperties", "EnableStreaming", "EntitiesOutput",
    "IgnoreGlobalQueryFilters", "IgnoreRowVersion", "IncludeGraph", "LoadOnlyIncludedColumns", "NotifyAfter",
    "NumberOfSkippedForUpdate", "OmitClauseExistsExcept", "OnConflictUpdateWhereSql", "OnSaveChangesSetFK",
    "PreserveInsertOrder", "PropertiesToExclude", "PropertiesToExcludeOnCompare", "PropertiesToExcludeOnUpdate",
    "PropertiesToInclude", "PropertiesToIncludeOnCompare", "PropertiesToIncludeOnUpdate", "ReplaceReadEntities",
    "SRID", "SetOutputIdentity", "SetOutputNonIdentityColumns", "ShadowPropertyValue", "SortOrder",
    "SqlBulkCopyColumnOrderHints", "SqlBulkCopyOptions", "StatsInfo", "TemporalColumns", "TrackingEntities",
    "UniqueTableNameTempDb", "UpdateByProperties", "UseOptionLoopJoin", "UseTempDB", "UseUnlogged", "WithHoldlock"
)

$ModelFeatures = [ordered]@{
    "OwnedType"          = "\.Owns(One|Many)\s*[<(]|\[Owned\]"
    "ValueConverter"     = "\.HasConversion\s*[<(]|ValueConverter<"
    "ShadowProperty"     = "\.Property\s*<[^>]+>\s*\(\s*""[^""]+""\s*\)"
    "RowVersion"         = "\.IsRowVersion\s*\(|\[Timestamp\]|IsConcurrencyToken\s*\("
    "ComputedColumn"     = "\.HasComputedColumnSql\s*\("
    "DefaultValue"       = "\.HasDefaultValue(Sql)?\s*\("
    "Discriminator_TPH"  = "\.HasDiscriminator\s*[<(]"
    "TPT_TPC"            = "UseTp[tc]MappingStrategy\s*\("
    "JsonColumn"         = "\.ToJson\s*\("
    "Spatial"            = "NetTopologySuite|UseNetTopologySuite"
    "HierarchyId"        = "HierarchyId"
    "TemporalTable"      = "\.IsTemporal\s*\("
    "GlobalQueryFilter"  = "\.HasQueryFilter\s*\("
    "TableSplitting"     = "\.SplitToTable\s*\("
    "Schema"             = "\.ToTable\s*\(\s*""[^""]+""\s*,\s*""[^""]+""\s*\)|\[Table\s*\([^)]*Schema"
    "Identity_HiLo_Seq"  = "UseIdentityColumn|UseHiLo|UseSequence|ValueGeneratedOnAdd\s*\("
    "SoftDelete_ABP"     = "ISoftDelete|IMayHaveTenant|IMustHaveTenant"
}

$ProviderPatterns = [ordered]@{
    "SqlServer"  = "UseSqlServer\s*\("
    "PostgreSql" = "UseNpgsql\s*\("
    "MySql"      = "UseMySql\s*\("
    "Sqlite"     = "UseSqlite\s*\("
    "Oracle"     = "UseOracle\s*\("
}

$ExcludeDirs = @("bin", "obj", ".git", ".vs", "node_modules", "packages", "TestResults", "wwwroot", "Migrations")
$ExcludeFileGlobs = @("appsettings*.json", "*.config", "*.pfx", "*.snk", "*.user", "*.min.js")

# ---------- helpers ----------
function Test-Excluded([string] $path) {
    foreach ($d in $ExcludeDirs) {
        if ($path -match "[\\/]" + [regex]::Escape($d) + "[\\/]") { return $true }
    }
    $name = Split-Path $path -Leaf
    foreach ($g in $ExcludeFileGlobs) {
        if ($name -like $g) { return $true }
    }
    return $false
}

function Get-RelPath([string] $full) {
    foreach ($r in $Root) {
        $rr = (Resolve-Path $r).Path.TrimEnd('\', '/')
        if ($full.StartsWith($rr, [System.StringComparison]::OrdinalIgnoreCase)) {
            return (Split-Path $rr -Leaf) + $full.Substring($rr.Length)
        }
    }
    return $full
}

function New-Hit([string] $file, [int] $line, [string] $text) {
    $t = $text.Trim()
    if ($t.Length -gt 220) { $t = $t.Substring(0, 220) + " ..." }
    return [pscustomobject]@{ file = (Get-RelPath $file); line = $line; code = $t }
}

function Add-Sample($bucket, $hit) {
    $bucket.count++
    if ($bucket.samples.Count -lt $MaxSamples) { $bucket.samples.Add($hit) | Out-Null }
    $bucket.files[$hit.file] = $true
}

function New-Bucket() {
    return @{ count = 0; samples = (New-Object System.Collections.ArrayList); files = @{} }
}

# ---------- collect files ----------
Write-Host "Scanning roots: $($Root -join ', ')"
$csFiles = @()
$csprojFiles = @()
foreach ($r in $Root) {
    if (-not (Test-Path -LiteralPath $r)) { throw "Root not found: $r" }
    $rootFull = (Resolve-Path -LiteralPath $r).Path.TrimEnd('\', '/')
    $all = Get-ChildItem -LiteralPath $rootFull -Recurse -File -ErrorAction SilentlyContinue
    # Exclusions apply to the path below the root, so a root that itself lives under e.g. "packages\" is not wiped out.
    $csFiles += $all | Where-Object { $_.Extension -eq ".cs" -and -not (Test-Excluded $_.FullName.Substring($rootFull.Length)) }
    $csprojFiles += $all | Where-Object { $_.Extension -in @(".csproj", ".props") -and -not (Test-Excluded $_.FullName.Substring($rootFull.Length)) }
}
Write-Host "  .cs files:     $($csFiles.Count)"
Write-Host "  .csproj/props: $($csprojFiles.Count)"

# ---------- projects ----------
$projects = New-Object System.Collections.ArrayList
$pkgRegex = '<PackageReference\s+Include="([^"]+)"(?:\s+Version="([^"]+)")?'
$pkgVersionRegex = '<PackageVersion\s+Include="([^"]+)"\s+Version="([^"]+)"'
$tfmRegex = '<TargetFrameworks?>([^<]+)</TargetFrameworks?>'
$interesting = "^(EFCore\.BulkExtensions|Microsoft\.EntityFrameworkCore|Npgsql|Pomelo|Abp|Volo\.Abp|Microsoft\.Data\.SqlClient)"

foreach ($p in $csprojFiles) {
    $content = Get-Content -Raw -LiteralPath $p.FullName
    $tfms = @()
    foreach ($m in [regex]::Matches($content, $tfmRegex)) { $tfms += ($m.Groups[1].Value -split ';') }
    $pkgs = @()
    foreach ($m in [regex]::Matches($content, $pkgRegex)) {
        if ($m.Groups[1].Value -match $interesting) {
            $pkgs += [pscustomobject]@{ id = $m.Groups[1].Value; version = $m.Groups[2].Value }
        }
    }
    foreach ($m in [regex]::Matches($content, $pkgVersionRegex)) {
        if ($m.Groups[1].Value -match $interesting) {
            $pkgs += [pscustomobject]@{ id = $m.Groups[1].Value; version = $m.Groups[2].Value }
        }
    }
    if ($tfms.Count -gt 0 -or $pkgs.Count -gt 0) {
        $projects.Add([pscustomobject]@{
            project  = (Get-RelPath $p.FullName)
            tfms     = ($tfms | ForEach-Object { $_.Trim() } | Where-Object { $_ })
            packages = $pkgs
        }) | Out-Null
    }
}

# ---------- scan .cs ----------
$methodBuckets = @{}; foreach ($m in $BulkMethods) { $methodBuckets[$m] = New-Bucket }
$optionBuckets = @{}; foreach ($o in $BulkConfigOptions) { $optionBuckets[$o] = New-Bucket }
$featureBuckets = @{}; foreach ($k in $ModelFeatures.Keys) { $featureBuckets[$k] = New-Bucket }
$providerBuckets = @{}; foreach ($k in $ProviderPatterns.Keys) { $providerBuckets[$k] = New-Bucket }
$usingBulk = New-Bucket
$bulkConfigNew = New-Bucket
$repoImpl = New-Bucket

$methodRegex = "\.(" + ($BulkMethods -join "|") + ")\s*(<[^>]*>)?\s*\("
$optionRegex = "\b(" + ($BulkConfigOptions -join "|") + ")\b"

$i = 0
foreach ($f in $csFiles) {
    $i++
    if ($i % 500 -eq 0) { Write-Host "  ... $i / $($csFiles.Count)" }
    $lines = @(Get-Content -LiteralPath $f.FullName -ErrorAction SilentlyContinue)
    if ($lines.Count -eq 0) { continue }
    $joined = [string]::Join("`n", $lines)

    # cheap pre-filter: skip files with no relevant token at all
    $relevant = ($joined -match "Bulk|Batch(Update|Delete)|Truncate|EFCore\.BulkExtensions|HasConversion|Owns(One|Many)|IsRowVersion|HasDiscriminator|UseTp[tc]|ToJson|NetTopologySuite|HierarchyId|IsTemporal|HasQueryFilter|SplitToTable|ToTable|Use(SqlServer|Npgsql|MySql|Sqlite|Oracle)\s*\(|ISoftDelete|IMayHaveTenant|IMustHaveTenant|HasComputedColumnSql|HasDefaultValue|UseIdentityColumn|UseHiLo|ValueGeneratedOnAdd|\[Timestamp\]|\[Owned\]|IsConcurrencyToken")
    if (-not $relevant) { continue }

    $isBulkFile = ($joined -match "EFCore\.BulkExtensions")
    # lambda bodies (opt => { opt.X = ...; }) span lines, so count options in any file that calls a Bulk method
    $isBulkCaller = $isBulkFile -or ($joined -match $methodRegex) -or ($joined -match "\bBulkConfig\b")

    for ($n = 0; $n -lt $lines.Count; $n++) {
        $ln = $lines[$n]
        if ([string]::IsNullOrWhiteSpace($ln)) { continue }
        $tl = $ln.TrimStart()
        if ($tl.StartsWith("//")) { continue }

        if ($ln -match "using\s+EFCore\.BulkExtensions") { Add-Sample $usingBulk (New-Hit $f.FullName ($n + 1) $ln) }
        if ($ln -match "IBulkRepository<|class\s+\w*BulkRepository") { Add-Sample $repoImpl (New-Hit $f.FullName ($n + 1) $ln) }
        if ($ln -match "new\s+BulkConfig\b|Action<BulkConfig>|\bBulkConfig\s+\w+\s*=") { Add-Sample $bulkConfigNew (New-Hit $f.FullName ($n + 1) $ln) }

        foreach ($m in [regex]::Matches($ln, $methodRegex)) {
            Add-Sample $methodBuckets[$m.Groups[1].Value] (New-Hit $f.FullName ($n + 1) $ln)
        }

        # option names are generic words (BatchSize, SortOrder...) -> only count when file references the lib
        # or the line clearly touches a BulkConfig instance
        if ($isBulkCaller) {
            foreach ($m in [regex]::Matches($ln, $optionRegex)) {
                $name = $m.Groups[1].Value
                # member access "x.Option =", "x.Option.Add(", "x.Option," / object initializer "Option =" / assignment continued on next line
                if ($ln -match ("\w+\s*\.\s*" + $name + "\s*(=[^=]|=\s*$|\)|,|;|\.)|\b" + $name + "\s*(=[^=]|=\s*$)")) {
                    Add-Sample $optionBuckets[$name] (New-Hit $f.FullName ($n + 1) $ln)
                }
            }
        }

        foreach ($k in $ModelFeatures.Keys) {
            if ($ln -match $ModelFeatures[$k]) { Add-Sample $featureBuckets[$k] (New-Hit $f.FullName ($n + 1) $ln) }
        }
        foreach ($k in $ProviderPatterns.Keys) {
            if ($ln -match $ProviderPatterns[$k]) { Add-Sample $providerBuckets[$k] (New-Hit $f.FullName ($n + 1) $ln) }
        }
    }
}

# ---------- emit ----------
New-Item -ItemType Directory -Force -Path $Out | Out-Null

function ConvertTo-Section($name, $buckets, $order) {
    $rows = @()
    foreach ($k in $order) {
        $b = $buckets[$k]
        if ($b.count -gt 0) {
            $rows += [pscustomobject]@{ name = $k; count = $b.count; files = $b.files.Count; samples = @($b.samples) }
        }
    }
    return $rows
}

$report = [ordered]@{
    generatedAt   = (Get-Date).ToString("s")
    roots         = $Root
    stats         = [ordered]@{ csFiles = $csFiles.Count; projectFiles = $csprojFiles.Count }
    projects      = @($projects)
    providers     = @(ConvertTo-Section "providers" $providerBuckets $ProviderPatterns.Keys)
    usingBulk     = [ordered]@{ count = $usingBulk.count; files = $usingBulk.files.Count; samples = @($usingBulk.samples) }
    repositories  = [ordered]@{ count = $repoImpl.count; files = $repoImpl.files.Count; samples = @($repoImpl.samples) }
    bulkConfigNew = [ordered]@{ count = $bulkConfigNew.count; files = $bulkConfigNew.files.Count; samples = @($bulkConfigNew.samples) }
    methods       = @(ConvertTo-Section "methods" $methodBuckets $BulkMethods)
    options       = @(ConvertTo-Section "options" $optionBuckets $BulkConfigOptions)
    modelFeatures = @(ConvertTo-Section "features" $featureBuckets $ModelFeatures.Keys)
}

$jsonPath = Join-Path $Out "report.json"
$report | ConvertTo-Json -Depth 8 | Set-Content -Path $jsonPath -Encoding UTF8

# markdown
$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("# EFCore.BulkExtensions usage scan")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("Generated: $($report.generatedAt)  ")
[void]$sb.AppendLine("Roots: " + ($Root -join ", ") + "  ")
[void]$sb.AppendLine("Files: $($csFiles.Count) .cs, $($csprojFiles.Count) .csproj/.props")
[void]$sb.AppendLine("")

[void]$sb.AppendLine("## Projects (TFM + relevant packages)")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("| Project | TFM | Packages |")
[void]$sb.AppendLine("|---|---|---|")
foreach ($p in $projects) {
    $pk = ($p.packages | ForEach-Object { if ($_.version) { "$($_.id) $($_.version)" } else { $_.id } }) -join "<br>"
    [void]$sb.AppendLine("| $($p.project) | $($p.tfms -join ';') | $pk |")
}
[void]$sb.AppendLine("")

function Write-CountTable($title, $rows) {
    [void]$sb.AppendLine("## $title")
    [void]$sb.AppendLine("")
    if ($rows.Count -eq 0) { [void]$sb.AppendLine("_none_"); [void]$sb.AppendLine(""); return }
    [void]$sb.AppendLine("| Name | Hits | Files |")
    [void]$sb.AppendLine("|---|---|---|")
    foreach ($r in $rows) { [void]$sb.AppendLine("| $($r.name) | $($r.count) | $($r.files) |") }
    [void]$sb.AppendLine("")
}

function Write-Samples($title, $rows) {
    [void]$sb.AppendLine("## $title (samples)")
    [void]$sb.AppendLine("")
    foreach ($r in $rows) {
        [void]$sb.AppendLine("### $($r.name) ($($r.count) hits)")
        [void]$sb.AppendLine("")
        [void]$sb.AppendLine('```')
        foreach ($s in $r.samples) { [void]$sb.AppendLine("$($s.file):$($s.line)  $($s.code)") }
        [void]$sb.AppendLine('```')
        [void]$sb.AppendLine("")
    }
}

Write-CountTable "DB providers" $report.providers
Write-CountTable "Bulk method call sites" $report.methods
Write-CountTable "BulkConfig options referenced" $report.options
Write-CountTable "EF model features affecting bulk mapping" $report.modelFeatures

[void]$sb.AppendLine("## Summary")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("- using EFCore.BulkExtensions: $($usingBulk.count) lines in $($usingBulk.files.Count) files")
[void]$sb.AppendLine("- BulkRepository / IBulkRepository references: $($repoImpl.count) lines in $($repoImpl.files.Count) files")
[void]$sb.AppendLine("- BulkConfig instantiations / lambdas: $($bulkConfigNew.count) lines in $($bulkConfigNew.files.Count) files")
[void]$sb.AppendLine("")

Write-Samples "Bulk method call sites" $report.methods
Write-Samples "BulkConfig options" $report.options
Write-Samples "BulkConfig instantiations" @([pscustomobject]@{ name = "BulkConfig"; count = $bulkConfigNew.count; samples = @($bulkConfigNew.samples) })
Write-Samples "EF model features" $report.modelFeatures
Write-Samples "DB providers" $report.providers

$mdPath = Join-Path $Out "report.md"
$sb.ToString() | Set-Content -Path $mdPath -Encoding UTF8

Write-Host ""
Write-Host "Done."
Write-Host "  $mdPath"
Write-Host "  $jsonPath"
Write-Host "Copy the '$Out' folder back to the Claude machine."
