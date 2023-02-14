using System;

namespace Gedaq.Npgsql.Enums
{
    [Flags]
    public enum SourceType
    {
        Connection = 1,
        NpgsqlDataSource = 2
    }
}