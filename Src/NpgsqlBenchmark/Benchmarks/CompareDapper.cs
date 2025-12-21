using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public partial class CompareDapper : PostgresBenchmark
    {
        private NpgsqlConnection _connection;

        [Params(0)]
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
            "GetAllPerson",
            typeof(Person)),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)
            ]
        [Benchmark(Baseline = true, Description = $"Gedaq.Npgsql", OperationsPerInvoke = 1_000)]
        public void Npgsql()
        {
            var persons = GetAllPerson(_connection, 50_000).ToList();
        }

        [Benchmark(Description = "Dapper", OperationsPerInvoke = 1_000)]
        public void Dapper()
        {
            var persons = _connection.Query<Person, Identification, Person>(@"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id,
    i.typename
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id = 50_000 },
splitOn: "identification_id"
)
                    .ToList();
        }

        [DapperAot]
        public static IEnumerable<Person> DapperAOTGetAllPerson(DbConnection connection, int id) => connection.Query<Person, Identification, Person>(
        @"SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id,
    i.typename
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id },
splitOn: "identification_id"
                );

        [Benchmark(Description = "DapperAOT", OperationsPerInvoke = 1_000)]
        public void DapperAOT()
        {
            var persons = DapperAOTGetAllPerson(_connection, 50_000).ToList();
        }
    }
}
