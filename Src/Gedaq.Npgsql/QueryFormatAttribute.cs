using System;

namespace Gedaq.Npgsql
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class QueryFormatAttribute : Attribute
    {
        public QueryFormatAttribute(
            string methodName,
            int position = 0,
            string parametrName = null
            )
        {
        }
    }
}