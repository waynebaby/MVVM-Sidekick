#if BLAZOR
using System;
using System.Linq.Expressions;

namespace MVVMSidekick.Views
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelMappingAttribute : Attribute
    {
        public string MapToProperty { get; set; }
        public bool Ignore { get; set; }

    }
}

#endif
