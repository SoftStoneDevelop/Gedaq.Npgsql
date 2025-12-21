using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using NpgsqlBenchmark.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net90)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public partial class CompareDapper : PostgresBenchmark
    {
        [Params(10, 20, 30)]
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
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = GetAllPerson(connection, 49999).ToList();
            }
        }

        [Benchmark(Description = "Dapper")]
        public async Task Dapper()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = connection.Query<Person, Identification, Person>(@"
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
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            for (int i = 0; i < Size; i++)
            {
                var persons = DapperAOTGetAllPerson(connection, 49999).ToList();
            }
        }
    }
}
