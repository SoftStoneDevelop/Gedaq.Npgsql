using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
using Gedaq.Provider.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class QueryReadAttribute : Attribute
    {
        public QueryReadAttribute(
            string query,
            Type queryMapType,
            MethodType methodType,
            SourceType sourceType,
            string methodName,
            bool generate = false
            )
        {
        }
    }
}