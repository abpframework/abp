using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MemoryDb.DependencyInjection
{
    public class AbpMemoryDbContextRegistrationOptions : AbpCommonDbContextRegistrationOptions, IAbpMemoryDbContextRegistrationOptionsBuilder
    {
        public AbpMemoryDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services) 
            : base(originalDbContextType, services)
        {
        }
    }
}