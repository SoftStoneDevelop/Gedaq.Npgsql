using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Gedaq.Common.Enums;
using Npgsql;
using System.Data.Common;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    public class PersonFlatBatch
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int IdentificationId { get; set; }
    }

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    public partial class CompareDapperBatch : PostgresBenchmark
    {
        private NpgsqlConnection _connection;

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
        public void IterationSetup()
        {
            _connection = _npgsqlDataSource.OpenConnection();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            try
            {
                _connection?.Dispose();
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

        [Benchmark(Baseline = true, Description = "Dapper")]
        public void Dapper()
        {
            var reader = _connection.QueryMultiple(@"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p;

SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p;
");
            var persons1 = reader.Read<PersonFlatBatch>().AsList();
            var persons2 = reader.Read<PersonFlatBatch>().AsList();
        }

        [DapperAot]
        public static SqlMapper.GridReader DapperAOTGetAllPerson(DbConnection connection) => connection.QueryMultiple(
        @"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p;

SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p;
");

        [Benchmark(Description = "DapperAOT")]
        public void DapperAOT()
        {
            var reader = DapperAOTGetAllPerson(_connection);
            var persons1 = reader.Read<PersonFlatBatch>().AsList();
            var persons2 = reader.Read<PersonFlatBatch>().AsList();
        }

        [Gedaq.Npgsql.Attributes.Query(
            query: @"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p
",
            methodName: "GetAllPerson",
            queryMapTypes: [typeof(PersonFlatBatch)],
            methodType: MethodType.Sync | MethodType.Async,
            asyncResultType: AsyncResult.ValueTask)]
        [Gedaq.Npgsql.Attributes.QueryBatch(
            batchName: "GetAllBatch",
            queryType: QueryType.Read,
            methodType: MethodType.Sync | MethodType.Async,
            asyncResultType: AsyncResult.ValueTask),
            Gedaq.Npgsql.Attributes.BatchPart("GetAllPerson", 0),
            Gedaq.Npgsql.Attributes.BatchPart("GetAllPerson", 1)]
        [Benchmark(Description = $"Gedaq Static Sync")]
        public void GedaqStatic()
        {
            var batchResult = GetAllBatch(_connection);
            var persons1 = batchResult[0];
            var persons2 = batchResult[1];
        }

        [Gedaq.Npgsql.Attributes.Query(
            query: null,
            methodName: "GetAllPersonDyn",
            queryMapTypes: [typeof(PersonFlatBatch)],
            methodType: MethodType.Sync | MethodType.Async,
            asyncResultType: AsyncResult.ValueTask)]
        [Gedaq.Npgsql.Attributes.QueryBatch(
            batchName: "GetAllBatchDyn",
            queryType: QueryType.Read,
            methodType: MethodType.Sync | MethodType.Async,
            asyncResultType: AsyncResult.ValueTask),
            Gedaq.Npgsql.Attributes.BatchPart("GetAllPersonDyn", 0),
            Gedaq.Npgsql.Attributes.BatchPart("GetAllPersonDyn", 1)]
        [Benchmark(Description = $"Gedaq Dynamic Sync")]
        public void GedaqDynamic()
        {
            var batchResult = GetAllBatchDyn(
                _connection,
                dynamicQuery0Batch: @"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p
",
                dynamicQuery1Batch: @"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id as IdentificationId
FROM person p
");
            var persons1 = batchResult[0];
            var persons2 = batchResult[1];
        }
    }
}
