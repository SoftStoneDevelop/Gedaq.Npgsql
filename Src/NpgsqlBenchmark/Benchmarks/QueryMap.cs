using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NpgsqlBenchmark.Model;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public class QueryMap : PostgresBenchmark
    {
        [Params(50, 100, 200)]
        public int Size;

        [GlobalSetup]
        public async Task Setup()
        {
            await OneTimeSetUp();
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            await OneTimeTearDown();
        }

        [Gedaq.Npgsql.Attributes.Query(
            @"
SELECT 
    p.id,
    p.firstname,
~StartInner::Identification:id~
    i.id,
    i.typename,
~EndInner::Identification~
    p.middlename,
    p.lastname
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id = $1
",
            "ReadInnerMap",
            typeof(Person)),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)
            ]
        [Benchmark(Description = $"Gedaq.Npgsql")]
        public async Task Npgsql()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = connection.ReadInnerMap(50000).ToList();
            }
        }

        [Gedaq.DbConnection.Attributes.Query(
            @"
SELECT 
    p.id,
    p.firstname,
~StartInner::Identification:id~
    i.id,
    i.typename,
~EndInner::Identification~
    p.middlename,
    p.lastname
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id = @id
",
            "ReadInnerMap",
            typeof(Person)
            ),
            Gedaq.DbConnection.Attributes.Parametr(parametrType: typeof(int), parametrName: "id")
            ]
        [Benchmark(Baseline = true, Description = "Gedaq.DbConnection")]
        public async Task DbConnection()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = ((DbConnection)connection).ReadInnerMap(50000).ToList();
            }
        }
    }
}
