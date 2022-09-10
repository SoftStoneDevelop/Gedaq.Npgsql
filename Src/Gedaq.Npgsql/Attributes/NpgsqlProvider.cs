using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class NpgsqlProvider : Gedaq.Provider.Attributes.ProviderAttribute
    {
    }
}