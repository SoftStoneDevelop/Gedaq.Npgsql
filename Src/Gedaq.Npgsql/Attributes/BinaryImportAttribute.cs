using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
using NpgsqlTypes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BinaryImportAttribute : Attribute
    {
        public BinaryImportAttribute(
            string query,
            string methodName,
            Type queryMapType,
            NpgsqlDbType[] dbTypes = null,
            MethodType methodType = MethodType.Sync,
            SourceType sourceType = SourceType.Connection,
            AccessModifier accessModifier = AccessModifier.AsContainingClass,
            AsyncResult asyncResultType = AsyncResult.ValueTask,
            Type asPartInterface = null)
        {
        }
    }
}