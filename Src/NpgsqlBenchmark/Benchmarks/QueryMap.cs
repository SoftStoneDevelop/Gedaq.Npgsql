using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Npgsql;
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
        private NpgsqlConnection _connection;

        [Params(50, 100, 200)]
        public int Calls;

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
WHERE p.id >= $1
",
            "ReadInnerMap",
            typeof(Person)),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)
            ]
        [Benchmark(Description = $"Gedaq.Npgsql")]
        public async Task Npgsql()
        {
            for (int i = 0; i < Calls; i++)
            {
                var persons = _connection.ReadInnerMap(999_999).ToList();
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
WHERE p.id >= @id
",
            "ReadInnerMap",
            typeof(Person)
            ),
            Gedaq.DbConnection.Attributes.Parametr(parametrType: typeof(int), parametrName: "id")
            ]
        [Benchmark(Baseline = true, Description = "Gedaq.DbConnection")]
        public async Task DbConnection()
        {
            for (int i = 0; i < Calls; i++)
            {
                var persons = ((DbConnection)_connection).ReadInnerMap(999_999).ToList();
            }
        }
    }
}
