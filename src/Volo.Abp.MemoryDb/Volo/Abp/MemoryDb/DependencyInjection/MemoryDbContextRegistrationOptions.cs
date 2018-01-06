using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MemoryDb.DependencyInjection
{
    public class MemoryDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IMemoryDbContextRegistrationOptionsBuilder
    {
        public MemoryDbContextRegistrationOptions(Type originalDbContextType) 
            : base(originalDbContextType)
        {
        }
    }
}