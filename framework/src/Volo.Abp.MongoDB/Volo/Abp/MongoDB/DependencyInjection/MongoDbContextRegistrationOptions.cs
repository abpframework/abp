using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class MongoDbContextRegistrationOptions : AbpCommonDbContextRegistrationOptions, IMongoDbContextRegistrationOptionsBuilder
    {
        public MongoDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services) 
            : base(originalDbContextType, services)
        {
        }
    }
}