using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.DependencyInjection
{
    public class DependencyAttribute : Attribute
    {
        [CanBeNull]
        public virtual ServiceLifetime? Lifetime { get; set; }

        public virtual bool TryRegister { get; set; }

        public virtual bool ReplaceServices { get; set; }

        public DependencyAttribute()
        {
            
        }

        public DependencyAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}