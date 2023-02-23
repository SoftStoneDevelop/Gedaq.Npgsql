using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BinaryExportAttribute : Attribute
    {
        public BinaryExportAttribute(
            string query,
            string methodName,
            Type queryMapType,
            MethodType methodType = MethodType.Sync,
            SourceType sourceType = SourceType.Connection
            )
        {
        }
    }
}