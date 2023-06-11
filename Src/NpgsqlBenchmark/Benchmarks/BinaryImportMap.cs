using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
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
    public class BinaryImportMap
    {
        [Params(10, 20, 30, 40)]
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
",
            "NpgsqlQuery",
            typeof(Person)
            )]
        [Benchmark(Description = $"NpgsqlQuery")]
        public void NpgsqlQuery()
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
        public void NpgsqlBinaryImport()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = _connection.NpgsqlBinaryImport().ToList();
            }
        }
    }
}
