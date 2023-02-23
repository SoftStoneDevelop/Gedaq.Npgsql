using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net70)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD")]
    public class QueryMap
    {
        [Params(50, 100, 200)]
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
WHERE p.id = $1
",
            "ReadInnerMap",
            typeof(Person)
            )]
        [Gedaq.Npgsql.Attributes.Parametr("ReadInnerMap", parametrType: typeof(int), position: 1)]
        [Benchmark(Description = $"Gedaq.Npgsql")]
        public void Npgsql()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = ((NpgsqlConnection)_connection).ReadInnerMap(50000).ToList();
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
WHERE p.id = @id
",
            "ReadInnerMap",
            typeof(Person)
            )]
        [Gedaq.DbConnection.Attributes.Parametr("ReadInnerMap", parametrType: typeof(int), parametrName:"id")]
        [Benchmark(Baseline = true, Description = "Gedaq.DbConnection")]
        public void DbConnection()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = ((DbConnection)_connection).ReadInnerMap(50000).ToList();
            }
        }
    }
}
