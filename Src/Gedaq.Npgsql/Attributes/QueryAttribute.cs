using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class QueryAttribute : Attribute
    {
        public QueryAttribute(
            string query,
            Type queryMapType,
            MethodType methodType,
            SourceType sourceType,
            string methodName,
            QueryType queryType = QueryType.Read,
            bool generate = true
            )
        {
        }
    }
}