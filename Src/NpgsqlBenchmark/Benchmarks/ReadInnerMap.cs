using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
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
    public class ReadInnerMap
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
        typeof(Person),
        MethodType.Sync,
        SourceType.Connection,
        "ReadInnerMap"
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
        typeof(Person),
        MethodType.Sync,
        "ReadInnerMap"
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
