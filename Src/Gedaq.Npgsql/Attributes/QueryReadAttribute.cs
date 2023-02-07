using Gedaq.Npgsql.Enums;
using Gedaq.Provider.Enums;

namespace Gedaq.Npgsql.Attributes
{
    public sealed class QueryReadAttribute : Gedaq.Provider.Attributes.QueryReadAttribute
    {
        public QueryReadAttribute(
            string[] queries,
            MethodType methodType,
            SourceType sourceType
            )
            : base(queries, methodType)
        {
        }

        public QueryReadAttribute(
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