using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextConventionalRegistrar : DefaultConventionalRegistrar
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IAbpEfCoreDbContext).IsAssignableFrom(type) || type == typeof(AbpDbContext<>))
            {
                return;
            }

            var replaceDbContextAttribute = type.GetCustomAttribute<ReplaceDbContextAttribute>(true);
            if (replaceDbContextAttribute == null)
            {
                return;
            }

            foreach (var dbContextType in replaceDbContextAttribute.ReplacedDbContextTypes)
            {
                services.Replace(
                    ServiceDescriptor.Transient(
                        dbContextType,
                        sp => sp.GetRequiredService(type)
                    )
                );

                services.Configure<AbpDbContextOptions>(opts => { opts.DbContextReplacements[dbContextType] = type; });
            }
        }
    }
}
