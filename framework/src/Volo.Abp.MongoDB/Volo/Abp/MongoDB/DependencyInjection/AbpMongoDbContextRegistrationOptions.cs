using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.DependencyInjection;

public class AbpMongoDbContextRegistrationOptions : AbpCommonDbContextRegistrationOptions, IAbpMongoDbContextRegistrationOptionsBuilder
{
    public AbpMongoDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        : base(originalDbContextType, services)
    {
    }
}
