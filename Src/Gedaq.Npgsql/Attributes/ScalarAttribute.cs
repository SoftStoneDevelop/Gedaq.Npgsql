﻿using System;

namespace Gedaq.Npgsql.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ScalarAttribute : Gedaq.Provider.Attributes.ScalarAttribute
    {
    }
}