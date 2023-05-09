using Gedaq.Common.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class QueryBatchAttribute : Attribute
    {
        public QueryBatchAttribute(
            string batchName,
            QueryType queryType,
            MethodType methodType,
            AccessModifier accessModifier = AccessModifier.AsContainingClass
            )
        {
        }
    }
}