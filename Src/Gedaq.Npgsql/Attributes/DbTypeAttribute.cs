using NpgsqlTypes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    /// <summary>
    /// An attribute indicating the type that corresponds to a property in the database
    /// </summary>
    /// <seealso href="https://github.com/SoftStoneDevelop/Gedaq.Npgsql/blob/main/Documentation/DbType.md"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="npgsqlDbType">Type in the database see <see cref="NpgsqlDbType"/></param>
        public DbTypeAttribute(NpgsqlDbType npgsqlDbType)
        {
        }
    }
}