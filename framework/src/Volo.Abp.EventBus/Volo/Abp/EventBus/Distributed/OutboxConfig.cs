using System;

namespace Volo.Abp.EventBus.Distributed
{
    public class OutboxConfig
    {
        public string Name { get; }
        
        public Type ImplementationType { get; set; }
        public Func<Type, bool> Selector { get; set; }

        public OutboxConfig(string name, Type implementationType, Func<Type, bool> selector = null)
        {
            Name = name;
            ImplementationType = implementationType;
        }
    }
}