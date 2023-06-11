using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.IO;
using System.Linq;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net70)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD", "Gen0", "Gen1", "Gen2")]
    public partial class ComparePrepareDapper
    {
        [Params(100, 200, 300)]
        public int Size;

        private NpgsqlConnection _connection;

        [GlobalSetup]
        public void Setup()
        {
            var root = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false)
                .Build()
                ;

            _connection = new NpgsqlConnection(root.GetConnectionString("SqlConnection"));
            _connection.Open();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _connection?.Dispose();
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
        public void Npgsql()
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
        public void Dapper()
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
