using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.Linq;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public partial class ComparePrepareDapper : PostgresBenchmark
    {
        private NpgsqlConnection _connection;

        [Params(100, 200, 300)]
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
            using var getAllCmd = CreateGetAllPersonCommand(_connection, prepare: true);
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
            var param = new Parametr();
            for (int i = 0; i < Size; i++)
            {
                param.id = i;
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
