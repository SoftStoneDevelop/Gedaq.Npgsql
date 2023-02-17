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
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net70)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD")]
    public class ReadInnerMapAsync
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
        MethodType.Async,
        SourceType.Connection,
        "ReadInnerMapAsync"
        )]
        [Gedaq.Npgsql.Attributes.Parametr("ReadInnerMapAsync", parametrType: typeof(int), position: 1)]
        [Benchmark(Description = $"Gedaq.Npgsql Async")]
        public async Task NpgsqlAsync()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = await ((NpgsqlConnection)_connection).ReadInnerMapAsyncAsync(50000).ToListAsync();
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
        MethodType.Async,
        "ReadInnerMapAsync"
        )]
        [Gedaq.DbConnection.Attributes.Parametr("ReadInnerMapAsync", parametrType: typeof(int), parametrName: "id")]
        [Benchmark(Baseline = true, Description = "Gedaq.DbConnection Async")]
        public async Task DbConnectionAsync()
        {
            for (int i = 0; i < Size; i++)
            {
                var persons = await ((DbConnection)_connection).ReadInnerMapAsyncAsync(50000).ToListAsync();
            }
        }
    }
}
