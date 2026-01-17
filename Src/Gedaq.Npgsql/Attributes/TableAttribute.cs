using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(
            string tableName,
            string schemaName,
            string tableAlias)
        {
        }
    }
}
