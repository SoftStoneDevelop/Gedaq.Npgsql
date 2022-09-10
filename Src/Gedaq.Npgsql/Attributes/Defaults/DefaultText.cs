namespace Gedaq.Npgsql.Attributes.Defaults
{
    public sealed class DefaultTextAttribute : Gedaq.Provider.Attributes.Constraints.DefaultAttribute
    {
        public DefaultTextAttribute(
            string constrainName,
            string defaultValue
            )
            : base(constrainName)
        {

        }
    }
}