using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class MongoDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IMongoDbContextRegistrationOptionsBuilder
    {
        public MongoDbContextRegistrationOptions(Type originalDbContextType) 
            : base(originalDbContextType)
        {
        }
    }
}