using NpgsqlTypes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    /// <summary>
    /// An attribute indicating the need to generate a binary export method.
    /// </summary>
    /// <seealso href="https://github.com/SoftStoneDevelop/Gedaq.Npgsql/blob/main/Documentation/DbTypesOverride.md"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DbTypesOverrideAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexOfTypeMap">The index of the type in queryMapTypes for which the DB types will be overridden.</param>
        /// <param name="dbTypes">The database types, in the order they appear in the query, that will be read in that order from the Row.</param>
        public DbTypesOverrideAttribute(
            int indexOfTypeMap,
            NpgsqlDbType[] dbTypes)
        {
        }
    }
}
