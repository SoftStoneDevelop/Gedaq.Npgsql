using BenchmarkDotNet.Running;
using NpgsqlBenchmark.Benchmarks;
using System.Threading.Tasks;

namespace NpgsqlBenchmark
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //BenchmarkRunner.Run<MarshallList>();
            // BenchmarkRunner.Run<CompareDapperBatch>();
            BenchmarkRunner.Run<CompareDapper>();
            // BenchmarkRunner.Run<QueryMap>();
            // BenchmarkRunner.Run<BinaryExportMap>();
        }
    }
}