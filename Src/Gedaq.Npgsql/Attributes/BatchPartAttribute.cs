using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BatchPartAttribute : Attribute
    {
        public BatchPartAttribute(
            string methodName,
            int position
            )
        {
        }
    }
}