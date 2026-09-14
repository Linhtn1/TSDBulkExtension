using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace TSD.BulkExtensions.Benchmarks;

/// <summary>
/// In-process, monitoring-style job: a few real runs per benchmark instead of statistical micro-benchmarking, because
/// every iteration is a database round trip of seconds. In-process also means a dev box without the .NET 6 runtime
/// rolls forward, which a generated child project would not.
/// </summary>
internal sealed class BenchConfig : ManualConfig
{
    public BenchConfig()
    {
        AddJob(Job.Default
            .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromMinutes(30), logOutput: true))
            .WithStrategy(RunStrategy.Monitoring)
            .WithWarmupCount(1)
            .WithIterationCount(3)
            .WithInvocationCount(1)
            .WithUnrollFactor(1));
    }
}
