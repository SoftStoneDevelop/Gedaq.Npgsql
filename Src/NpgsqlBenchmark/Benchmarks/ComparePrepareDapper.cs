using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using NpgsqlBenchmark.Model;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net90)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public partial class ComparePrepareDapper : PostgresBenchmark
    {
        [Params(100, 200, 300)]
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
WHERE p.id < $1
",
            "GetAllPerson",
            typeof(Person)
            ),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1) 
            ]
        [Benchmark(Description = $"Gedaq.Npgsql")]
        public async Task Npgsql()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            using var getAllCmd = CreateGetAllPersonCommand(connection, prepare: true);
            for (int i = 0; i < Size; i++)
            {
                SetGetAllPersonParametrs(getAllCmd, i);
                var persons = ExecuteGetAllPersonCommand(getAllCmd).ToList();
            }
        }

        public class Parametr
        {
            public int id { get; set; }
        }

        [Benchmark(Baseline = true, Description = "Dapper")]
        public async Task Dapper()
        {
            await using var connection = await _npgsqlDataSource.OpenConnectionAsync();
            var param = new Parametr();
            for (int i = 0; i < Size; i++)
            {
                param.id = i;
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
WHERE p.id < @id
", 
(person, ident) => 
{
    person.Identification = ident;
    return person;
},
param,
splitOn: "identification_id"
)
                    .ToList();
            }
        }
    }
}
