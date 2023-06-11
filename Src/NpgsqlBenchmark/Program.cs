using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlBenchmark.Benchmarks;
using System;
using System.IO;

namespace NpgsqlBenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //FillTestDatabase();
            BenchmarkRunner.Run<ComparePrepareDapper>();
            //BenchmarkRunner.Run<CompareDapper>();
            //BenchmarkRunner.Run<QueryMap>();
            //BenchmarkRunner.Run<BinaryImportMap>();
        }

        private static void FillTestDatabase()
        {
            var root = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false)
                .Build()
                ;

            using var connection = new NpgsqlConnection(root.GetConnectionString("SqlConnection"));
            connection.Open();
            CreateIdentificationTable(connection);
            CreatePersonTable(connection);
            FillIndetification(connection);
            FillPerson(connection);
        }

        private static void CreateIdentificationTable(NpgsqlConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS public.identification
(
    id integer NOT NULL,
    typename text COLLATE pg_catalog.""default"" NOT NULL,
    CONSTRAINT identification_pkey PRIMARY KEY (id)
)
";
            cmd.ExecuteNonQuery();
        }

        private static void CreatePersonTable(NpgsqlConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS public.person
(
    id integer NOT NULL,
    firstname text COLLATE pg_catalog.""default"",
    middlename text COLLATE pg_catalog.""default"",
    lastname text COLLATE pg_catalog.""default"",
    identification_id integer,
    CONSTRAINT person_pkey PRIMARY KEY (id),
    CONSTRAINT identification_fk FOREIGN KEY (identification_id)
        REFERENCES public.identification (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)
";
            cmd.ExecuteNonQuery();
        }

        private static void FillIndetification(NpgsqlConnection connection)
        {
            using var cmd = connection.CreateCommand();
            {
                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS public.identification
(
    id integer NOT NULL,
    typename text COLLATE pg_catalog.""default"" NOT NULL,
    CONSTRAINT identification_pkey PRIMARY KEY (id)
)
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
INSERT INTO public.identification(
	id, typename)
	VALUES (
    @id, @typename
);
";
                var id = cmd.CreateParameter();
                id.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
                id.ParameterName = "id";
                cmd.Parameters.Add(id);

                var typename = cmd.CreateParameter();
                typename.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text;
                typename.ParameterName = "typename";
                cmd.Parameters.Add(typename);
                cmd.Prepare();

                id.Value = 1;
                typename.Value = "sailor's passport";
                cmd.ExecuteNonQuery();

                id.Value = 2;
                typename.Value = "officer's certificate";
                cmd.ExecuteNonQuery();

                id.Value = 3;
                typename.Value = "driver license";
                cmd.ExecuteNonQuery();

                id.Value = 4;
                typename.Value = "citizen's passport";
                cmd.ExecuteNonQuery();

                id.Value = 5;
                typename.Value = "party card";
                cmd.ExecuteNonQuery();
            }
        }

        private static void FillPerson(NpgsqlConnection connection)
        {
            using (var writer = connection.BeginBinaryImport(@"
COPY person (
    id,
    firstname,
    middlename,
    lastname,
    identification_id
)
FROM STDIN (FORMAT BINARY)
"))
            {
                writer.Timeout = new TimeSpan(0, 30, 0);
                var refId = 0;
                var setNull = false;
                var millions = 0;
                var millionsCounter = 0;

                for (int i = 0; i < 100_000; i++)
                {
                    writer.StartRow();
                    writer.Write(i, NpgsqlTypes.NpgsqlDbType.Integer);
                    writer.Write($"John{i}", NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write($"Сurly{i}", NpgsqlTypes.NpgsqlDbType.Text);
                    writer.Write($"Doe{i}", NpgsqlTypes.NpgsqlDbType.Text);

                    if (++refId > 5)
                    {
                        refId = 1;
                        setNull = true;
                    }

                    if (setNull)
                    {
                        writer.WriteNull();
                        setNull = false;
                    }
                    else
                    {
                        writer.Write(refId, NpgsqlTypes.NpgsqlDbType.Integer);
                    }

                    if (++millionsCounter == 1_000_000)
                    {
                        millionsCounter = 0;
                        Console.WriteLine($"{++millions} Million");
                    }
                }
                Console.WriteLine($"Start Complete");
                writer.Complete();
                Console.WriteLine($"Complete sucsess");
            }
        }
    }
}