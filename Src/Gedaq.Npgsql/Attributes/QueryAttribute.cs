using Gedaq.Npgsql.Enums;
using Gedaq.Provider.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class QueryAttribute : Gedaq.Provider.Attributes.QueryReadAttribute
    {
        public QueryAttribute(
            (
            string query,
            string itemTypeName
            )[] queries,
            MethodType methodType,
            SourceType sourceType
            )
            : base(queries, methodType)
        {
        }

        public QueryAttribute(
            (
            string query,
            NpgsqlTypes.NpgsqlDbType[] returnTypes,
            string itemTypeName
            )[] queries,
            MethodType methodType,
            SourceType sourceType
            )
        {
        }
    }
}