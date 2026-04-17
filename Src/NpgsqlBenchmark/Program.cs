using BenchmarkDotNet.Running;
using NpgsqlBenchmark.Benchmarks;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NpgsqlBenchmark
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new DefaultInterpolatedStringHandler();
            builder.AppendLiteral("sddfggdffg");
            var text = builder.Text;

            //BenchmarkRunner.Run<ComparePrepareDapper>();
            //BenchmarkRunner.Run<CompareDapper>();
            var s = new QueryMap();
            await s.GlobalSetup();

            s.Calls = 10;
            s.IterationSetup();

            await s.Npgsql();

            s.IterationCleanup();

            await s.GlobalCleanup();

            //BenchmarkRunner.Run<QueryMap>();
            //BenchmarkRunner.Run<BinaryImportMap>();
        }
    }
}