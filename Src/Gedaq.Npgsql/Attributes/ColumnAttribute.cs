using NpgsqlTypes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ColumnAttribute : Attribute
    {
        public ColumnAttribute(
            string columnName,
            string columnAlias,
            NpgsqlDbType dbType)
        {
        }
    }
}
