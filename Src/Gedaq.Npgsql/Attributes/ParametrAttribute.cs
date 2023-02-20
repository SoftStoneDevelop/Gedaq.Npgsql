using NpgsqlTypes;
using System;
using System.Data;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ParametrAttribute : Attribute
    {
        public ParametrAttribute(
            string methodName,
            Type parametrType,
            string parametrName = null,
            NpgsqlDbType dbType = NpgsqlDbType.Unknown,
            int size = -1,
            bool nullable = false,
            ParameterDirection direction = ParameterDirection.Input,
            int position = -1,
            string sourceColumn = "",
            bool sourceColumnNullMapping = false,
            DataRowVersion sourceVersion = DataRowVersion.Current,
            byte scale = 0,
            byte precision = 0
            )
        {
        }
    }
}