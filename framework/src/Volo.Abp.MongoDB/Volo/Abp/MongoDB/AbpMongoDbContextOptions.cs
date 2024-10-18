using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.MongoDB;

public class AbpMongoDbContextOptions
{
    internal Dictionary<MultiTenantDbContextType, Type> DbContextReplacements { get; }

    public Action<MongoClientSettings>? MongoClientSettingsConfigurer { get; set; }

    public AbpMongoDbContextOptions()
    {
        DbContextReplacements = new Dictionary<MultiTenantDbContextType, Type>();
    }

    internal Type GetReplacedTypeOrSelf(Type dbContextType, MultiTenancySides multiTenancySides = MultiTenancySides.Both)
    {
        var replacementType = dbContextType;
        while (true)
        {
            var foundType = DbContextReplacements.LastOrDefault(x => x.Key.Type == replacementType && x.Key.MultiTenancySide.HasFlag(multiTenancySides));
            if (!foundType.Equals(default(KeyValuePair<MultiTenantDbContextType, Type>)))
            {
                if (foundType.Value == dbContextType)
                {
                    throw new AbpException(
                        "Circular DbContext replacement found for " +
                        dbContextType.AssemblyQualifiedName
                    );
                }
                replacementType = foundType.Value;
            }
            else
            {
                return replacementType;
            }
        }
    }
}
