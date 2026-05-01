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
    public class BinaryExportMap : PostgresBenchmark
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
        public void IterationSetup()
        {
            _connection = _npgsqlDataSource.OpenConnection();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            try
            {
                _connection?.Dispose();
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
            query: @"
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
            methodName: "NpgsqlQuery",
            queryMapTypes: [typeof(Person)])]
        [Benchmark(Description = $"NpgsqlQuery")]
        public async Task NpgsqlQuery()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = _connection.NpgsqlQuery();
            }
        }

        [Gedaq.Npgsql.Attributes.BinaryExport(
            query: @"
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
            methodName: "NpgsqlBinaryExport",
            queryMapTypes: [typeof(Person)])]
        [Benchmark(Baseline = true, Description = "NpgsqlBinaryExport")]
        public async Task NpgsqlBinaryExport()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = _connection.NpgsqlBinaryExport().ToList();
            }
        }
    }
}
