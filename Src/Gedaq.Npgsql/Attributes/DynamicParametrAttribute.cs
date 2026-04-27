using System;

namespace Gedaq.Npgsql.Attributes
{
    /// <summary>
    /// An attribute indicating that the parameters in a method are passed dynamically and are unknown at build time.
    /// </summary>
    /// <seealso cref="https://github.com/SoftStoneDevelop/Gedaq.Npgsql/blob/main/Documentation/DynamicParametr.md"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DynamicParametrAttribute : Attribute
    {
        public DynamicParametrAttribute()
        {
        }
    }
}
