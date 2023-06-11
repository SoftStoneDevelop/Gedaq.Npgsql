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
    public class CompareDapper
    {
        [Params(10, 20, 30)]
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
WHERE p.id > $1
",
            "CompareRead",
            typeof(Person)
            ),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)
            ]
        [Benchmark(Description = $"Gedaq.Npgsql")]
        public void Npgsql()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = ((NpgsqlConnection)_connection).CompareRead(49999).ToList();
            }
        }

        [Benchmark(Baseline = true, Description = "Dapper")]
        public void Dapper()
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
    }
}
