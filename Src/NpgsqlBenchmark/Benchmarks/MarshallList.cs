using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    public partial class MarshallList : PostgresBenchmark
    {
        [Params(50, 100, 500, 1000)]
        public int Items;

        private static Task<object> GetNewObject()
        {
            return Task.FromResult(new object());
        }

        [Benchmark(Baseline = true, Description = "List.Add")]
        public async Task Simple()
        {
            var list = new List<object>();
            for (int i = 0; i < Items; i++)
            {
                var newObject = await GetNewObject();
                list.Add(newObject);
            }
        }

        [Benchmark(Description = "CollectionsMarshal")]
        public async Task Marshall()
        {
            var list = new List<object>();
            for (int i = 0; i < Items; i++)
            {
                var newObject = await GetNewObject();

                var span = CollectionsMarshal.AsSpan(list);
                if (span.Length <= i)
                {
                    CollectionsMarshal.SetCount(list, i + 1);
                    span = CollectionsMarshal.AsSpan(list);
                }

                span[i] = newObject;
            }
        }
    }
}
