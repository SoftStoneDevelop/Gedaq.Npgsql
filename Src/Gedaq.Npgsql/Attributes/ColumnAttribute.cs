using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ColumnAttribute : Gedaq.Provider.Attributes.ColumnAttribute
    {
        public ColumnAttribute(
            string nameInDatabase,
            NpgsqlTypes.NpgsqlDbType type
            )
            : base(nameInDatabase)
        {
        }
    }
}