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

        [Params(10, 20, 30)]
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
WHERE p.id > $1
",
            "GetAllPerson",
            typeof(Person)
            ),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)
            ]
        [Benchmark(Baseline = true, Description = $"Gedaq.Npgsql")]
        public async Task Npgsql()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = GetAllPerson(_connection, 49999).ToList();
            }
        }

        [Benchmark(Description = "Dapper")]
        public async Task Dapper()
        {
            for (int i = 0; i < Size; i++)
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
WHERE p.id > @id
", 
(person, ident) => 
{
    person.Identification = ident;
    return person;
},
new { id = 49999 },
splitOn: "identification_id"
)
                    .ToList();
            }
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
WHERE p.id > @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id },
splitOn: "identification_id"
                );

        [Benchmark(Description = "DapperAOT")]
        public async Task DapperAOT()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = DapperAOTGetAllPerson(_connection, 49999).ToList();
            }
        }
    }
}
