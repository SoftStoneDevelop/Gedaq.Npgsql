using Gedaq.Provider.Attributes;
using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class NpgsqlTableAttribute : TableAttribute
    {
        public NpgsqlTableAttribute(string nameInDatabase) : base(nameInDatabase)
        {
        }
    }
}