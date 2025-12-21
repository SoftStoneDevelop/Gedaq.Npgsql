using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NpgsqlBenchmark.Model;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net90)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public class BinaryImportMap : PostgresBenchmark
    {
        [Params(10, 20, 30, 40)]
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
",
            "NpgsqlQuery",
            typeof(Person)
            )]
        [Benchmark(Description = $"NpgsqlQuery")]
        public async Task NpgsqlQuery()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = connection.NpgsqlQuery().ToList();
            }
        }

        [Gedaq.Npgsql.Attributes.BinaryExport(
            @"
COPY 
(
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
~StartInner::Identification:id~
    i.id,
    i.typename
~EndInner::Identification~
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
) TO STDOUT (FORMAT BINARY)
",
            "NpgsqlBinaryImport",
            typeof(Person)
            )]
        [Benchmark(Baseline = true, Description = "NpgsqlBinaryImport")]
        public async Task NpgsqlBinaryImport()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = connection.NpgsqlBinaryImport().ToList();
            }
        }
    }
}
