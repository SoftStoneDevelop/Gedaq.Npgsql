
using Npgsql;
using Npgsql.Schema;
using NpgsqlBenchmark.Benchmarks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace NpgsqlBenchmark.Benchmarks2
{
    public static class QueryMapReadInnerMapNpgsqlExtension3
    {
        private static string ReadInnerMapColumnToPropertyName(string columnName)
        {
            switch (columnName)
            {
                case "id":
                    {
                        return "id";
                    }


                case "firstname":
                    {
                        return "firstname";
                    }

                case "middlename":
                    {
                        return "middlename";
                    }

                case "lastname":
                    {
                        return "lastname";
                    }

                case "identification_id":
                    {
                        return "identification_id";
                    }

                case "identification_typename":
                    {
                        return "identification_typename";
                    }

                default:
                {
                    return columnName;
                }
            }
        }

        private static void ReadInnerMapSetPropertyValue(
            NpgsqlDbColumn column,
            string propertyName,
            NpgsqlDataReader reader,
            NpgsqlBenchmark.Model.Person root)
        {
            switch (propertyName)
            {
                case "id":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            root.Id = reader.GetFieldValue<System.Int32>(column.ColumnName);
                        }

                        break;
                    }

                case "firstname":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            root.FirstName = reader.GetFieldValue<System.String>(column.ColumnName);
                        }

                        break;
                    }

                case "middlename":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            root.MiddleName = reader.GetFieldValue<System.String>(column.ColumnName);
                        }

                        break;
                    }

                case "lastname":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            root.LastName = reader.GetFieldValue<System.String>(column.ColumnName);
                        }

                        break;
                    }

                case "identification_id":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            if (root.Identification == null)
                            {
                                root.Identification = new NpgsqlBenchmark.Model.Identification();
                            }

                            root.Identification.Id = reader.GetFieldValue<System.Int32>(column.ColumnName);
                        }

                        break;
                    }

                case "identification_typename":
                    {
                        if (!reader.IsDBNull(column.ColumnName))
                        {
                            if (root.Identification == null)
                            {
                                root.Identification = new NpgsqlBenchmark.Model.Identification();
                            }

                            root.Identification.TypeName = reader.GetFieldValue<System.String>(column.ColumnName);
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }


        public static IEnumerable<NpgsqlBenchmark.Model.Person> ReadInnerMap(
            this Npgsql.NpgsqlConnection connection,
            string query,
            NpgsqlParameter[] parameters,
            int? timeout = null
        )
        {
            bool needClose = connection.State == ConnectionState.Closed;
            if (needClose)
            {
                connection.Open();
            }
            NpgsqlCommand command = null;
            NpgsqlDataReader reader = null;
            try
            {
                command =
                CreateReadInnerMapCommand(connection, query, false)
                ;
                command.SetReadInnerMapParametrs(
                    parameters,
                    timeout
                    );
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NpgsqlBenchmark.Model.Person item;
                    var root = new NpgsqlBenchmark.Model.Person();

                    var columns = reader.GetColumnSchema();
                    foreach (var column in columns)
                    {
                        var propertyName = ReadInnerMapColumnToPropertyName(column.ColumnName);
                        ReadInnerMapSetPropertyValue(column, propertyName, reader, root);
                    }

                    item = root;
                    yield return item;
                }

                while (reader.NextResult())
                {
                }
                reader.Dispose();
                reader = null;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        try
                        {
                            command.Cancel();
                        }
                        catch { /* ignore */ }
                    }

                    reader.Dispose();
                }
                if (needClose)
                {
                    connection.Close();
                }
                if (command != null)
                {
                    command.Parameters.Clear();
                    command.Dispose();
                }
            }
        }

        public static NpgsqlCommand CreateReadInnerMapCommand(
            this Npgsql.NpgsqlConnection connection,
            string query,
            bool prepare = false
        )
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            {
                var parametr = new NpgsqlParameter<System.Int32>();

                command.Parameters.Add(parametr);

            }
            if (prepare)
            {
                try
                {
                    command.Prepare();
                }
                catch
                {
                    command.Dispose();
                    throw;
                }
            }
            return command;
        }

        public static IEnumerable<NpgsqlBenchmark.Model.Person> ExecuteReadInnerMapCommand(
            this NpgsqlCommand command
            )
        {
            NpgsqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NpgsqlBenchmark.Model.Person item;
                    var root = new NpgsqlBenchmark.Model.Person();
                    if (!reader.IsDBNull(0))
                    {
                        if (root == null)
                        {
                            root = new NpgsqlBenchmark.Model.Person();
                        }
                        root.Id = reader.GetFieldValue<System.Int32>(0);
                    }
                    if (!reader.IsDBNull(1))
                    {
                        if (root == null)
                        {
                            root = new NpgsqlBenchmark.Model.Person();
                        }
                        root.FirstName = reader.GetFieldValue<System.String>(1);
                    }
                    if (!reader.IsDBNull(2))
                    {
                        var item1 = new NpgsqlBenchmark.Model.Identification();
                        if (!reader.IsDBNull(2))
                        {
                            if (item1 == null)
                            {
                                item1 = new NpgsqlBenchmark.Model.Identification();
                            }
                            item1.Id = reader.GetFieldValue<System.Int32>(2);
                        }
                        if (!reader.IsDBNull(3))
                        {
                            if (item1 == null)
                            {
                                item1 = new NpgsqlBenchmark.Model.Identification();
                            }
                            item1.TypeName = reader.GetFieldValue<System.String>(3);
                        }
                        root.Identification = item1;
                    }
                    if (!reader.IsDBNull(4))
                    {
                        if (root == null)
                        {
                            root = new NpgsqlBenchmark.Model.Person();
                        }
                        root.MiddleName = reader.GetFieldValue<System.String>(4);
                    }
                    if (!reader.IsDBNull(5))
                    {
                        if (root == null)
                        {
                            root = new NpgsqlBenchmark.Model.Person();
                        }
                        root.LastName = reader.GetFieldValue<System.String>(5);
                    }
                    item = root;
                    yield return item;
                }

                while (reader.NextResult())
                {
                }
                reader.Dispose();
                reader = null;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        try
                        {
                            command.Cancel();
                        }
                        catch { /* ignore */ }
                    }

                    reader.Dispose();
                }
            }
        }

        public static void SetReadInnerMapParametrs(
            this NpgsqlCommand command,
            NpgsqlParameter[] parameters,
            int? timeout = null
            )
        {
            if (timeout.HasValue)
            {
                command.CommandTimeout = timeout.Value;
            }

            command.Parameters.AddRange(parameters);
        }

    }
}