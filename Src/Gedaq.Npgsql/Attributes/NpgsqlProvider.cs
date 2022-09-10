using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class NpgsqlProvider : Gedaq.Provider.Attributes.ProviderAttribute
    {
    }
}