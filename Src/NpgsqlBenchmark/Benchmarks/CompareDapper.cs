using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Gedaq.Common.Enums;
using Npgsql;
using NpgsqlBenchmark.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks
{
    public class PersonFlat
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [Gedaq.Common.Attributes.IgnoreProperty()]
        public Identification Identification { get; set; }
    }

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    public partial class CompareDapper : PostgresBenchmark
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
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id = 50_000 },
splitOn: "identification_id").AsList();
        }

        [Benchmark(Description = "Dapper Async")]
        public async Task DapperAsync()
        {
            var persons = (await _connection.QueryAsync<Person, Identification, Person>(@"
SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id,
    i.typename
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id = 50_000 },
splitOn: "identification_id")).AsList();
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
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id },
splitOn: "identification_id");

        [Benchmark(Description = "DapperAOT")]
        public void DapperAOT()
        {
            var persons = DapperAOTGetAllPerson(_connection, 50_000).AsList();
        }

        [DapperAot]
        public static Task<IEnumerable<Person>> DapperAOTGetAllPersonAsync(DbConnection connection, int id) => connection.QueryAsync<Person, Identification, Person>(
        @"SELECT 
    p.id,
    p.firstname,
    p.middlename,
    p.lastname,
    p.identification_id,
    i.typename
FROM person p
LEFT JOIN identification i ON i.id = p.identification_id
WHERE p.id >= @id
",
(person, ident) =>
{
    person.Identification = ident;
    return person;
},
new { id },
splitOn: "identification_id");

        [Benchmark(Description = "DapperAOT Async")]
        public async Task DapperAOTAsync()
        {
            var persons = (await DapperAOTGetAllPersonAsync(_connection, 50_000)).AsList();
        }

        [Gedaq.Npgsql.Attributes.Query(
            query: @"
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
WHERE p.id >= $1
",
            methodName: "GetAllPerson",
            queryMapTypes: [typeof(Person)],
            methodType: MethodType.Sync | MethodType.Async),
            Gedaq.Npgsql.Attributes.Parametr(parametrType: typeof(int), position: 1)]
        [Benchmark(Description = $"Gedaq Static Sync")]
        public void GedaqStatic()
        {
            var persons = GetAllPerson(_connection, 50_000);
        }

        [Benchmark(Description = $"Gedaq Static Async")]
        public async Task GedaqStaticAsync()
        {
            var persons = await GetAllPersonAsync(_connection, 50_000);
        }

        [Gedaq.Npgsql.Attributes.Query(
            query: null,
            methodName: "GetAllPersonDyn",
            queryMapTypes: [typeof(PersonFlat), typeof(Identification)],
            overrideAliasPrefixs: ["person_", "identity_"],
            methodType: MethodType.Sync | MethodType.Async),
            Gedaq.Npgsql.Attributes.DynamicParametr()]
        [Benchmark(Description = $"Gedaq Dynamic Sync")]
        public void GedaqDynamic()
        {
            var persons = new List<PersonFlat>();
            var parametr = new NpgsqlParameter<int>();
            parametr.TypedValue = 50_000;

            GetAllPersonDyn(_connection, @"
        SELECT 
            p.id as person_id,
            p.firstname as person_firstname,
            p.middlename as person_middlename,
            p.lastname as person_lastname,
            i.id as identity_id,
            i.typename as identity_typename
        FROM person p
        LEFT JOIN identification i ON i.id = p.identification_id
        WHERE p.id >= $1
        ",
[parametr],
(person, indetity) => { person.Identification = indetity; persons.Add(person); });
        }

        [Benchmark(Description = $"Gedaq Dynamic Async")]
        public async Task GedaqDynamicAsync()
        {
            var persons = new List<PersonFlat>();
            var parametr = new NpgsqlParameter<int>();
            parametr.TypedValue = 50_000;

            await GetAllPersonDynAsync(_connection, @"
        SELECT 
            p.id as person_id,
            p.firstname as person_firstname,
            p.middlename as person_middlename,
            p.lastname as person_lastname,
            i.id as identity_id,
            i.typename as identity_typename
        FROM person p
        LEFT JOIN identification i ON i.id = p.identification_id
        WHERE p.id >= $1
        ",
[parametr],
(person, indetity) => { person.Identification = indetity; persons.Add(person); });
        }
    }
}
