using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public class BinaryImportMap : PostgresBenchmark
    {
        private NpgsqlConnection _connection;

        [Params(10, 20, 30, 40)]
        public int Size;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await OneTimeSetUp();
        }

        [GlobalCleanup]
        public async Task GlobalCleanup()
        {
            await OneTimeTearDown();
        }

        [IterationSetup]
        public async Task IterationSetup()
        {
            _connection = await _npgsqlDataSource.OpenConnectionAsync();
        }

        [IterationCleanup]
        public async Task IterationCleanup()
        {
            try
            {
                var connection = _connection;
                if (connection != null)
                {
                    await connection.DisposeAsync();
                }
            }
            catch
            {
                // ignore
            }
            finally
            {
                _connection = null;
            }
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
            for (int i = 0; i < Size; i++)
            {
                var persons = _connection.NpgsqlQuery().ToList();
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
            for (int i = 0; i < Size; i++)
            {
                var persons = _connection.NpgsqlBinaryImport().ToList();
            }
        }
    }
}
