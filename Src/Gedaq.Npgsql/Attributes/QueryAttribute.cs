using Gedaq.Npgsql.Enums;
using Gedaq.Provider.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class QueryAttribute : Gedaq.Provider.Attributes.QueryAttribute
    {
        public QueryAttribute(
            (
            string query,
            string itemTypeName,
            ResultType resultMethodType
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
            string itemTypeName,
            ResultType resultMethodType
            )[] queries,
            MethodType methodType,
            SourceType sourceType
            )
        {
        }
    }
}