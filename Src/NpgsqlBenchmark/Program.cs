using BenchmarkDotNet.Running;
using NpgsqlBenchmark.Benchmarks;

namespace NpgsqlBenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<ComparePrepareDapper>();
            BenchmarkRunner.Run<CompareDapper>();
            //BenchmarkRunner.Run<QueryMap>();
            //BenchmarkRunner.Run<BinaryImportMap>();
        }
    }
}