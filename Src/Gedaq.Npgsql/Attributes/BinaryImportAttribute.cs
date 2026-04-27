using Gedaq.Common.Enums;
using Gedaq.Npgsql.Enums;
using NpgsqlTypes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    /// <summary>
    /// An attribute indicating the need to generate a binary import method.
    /// </summary>
    /// <seealso href="https://github.com/SoftStoneDevelop/Gedaq.Npgsql/blob/main/Documentation/BinaryImport.md"/>
    /// <seealso href="https://www.npgsql.org/doc/api/Npgsql.NpgsqlBinaryImporter.html"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BinaryImportAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">Sql query</param>
        /// <param name="methodName">Name of the generated method</param>
        /// <param name="queryMapType">The data type of the collection of elements from which the import will be performed</param>
        /// <param name="dbTypes">The database types, in the order they appear in the query, that will be read in that order from the Row.</param>
        /// <param name="methodType">Type of generated method see <see cref="MethodType"/></param>
        /// <param name="sourceType">The type of database connection source for which the method will be generated see <see cref="SourceType"/></param>
        /// <param name="accessModifier">Access Modifier of Generated Methods see <see cref="AccessModifier"/></param>
        /// <param name="asyncResultType">The type of the generated Task/ValueTask see <see cref="AsyncResult"/> method</param>
        /// <param name="asPartInterface">The interface of which the generated method should be a part. It also generates descriptions of this method for the interface.</param>
        public BinaryImportAttribute(
            string query,
            string methodName,
            Type queryMapType,
            NpgsqlDbType[]? dbTypes = null,
            MethodType methodType = MethodType.Sync,
            SourceType sourceType = SourceType.Connection,
            AccessModifier accessModifier = AccessModifier.AsContainingClass,
            AsyncResult asyncResultType = AsyncResult.ValueTask,
            Type? asPartInterface = null)
        {
        }
    }
}