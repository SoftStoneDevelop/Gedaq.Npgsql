using Gedaq.Common.Enums;
using Gedaq.Provider.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class QueryBatchAttribute : Attribute
    {
        public QueryBatchAttribute(
            string batchName,
            BatchType batchType,
            MethodType methodType
            )
        {
        }
    }
}