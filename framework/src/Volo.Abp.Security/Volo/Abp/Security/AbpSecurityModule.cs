using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Security;

public class AbpSecurityModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddClaimsPrincipalContributors(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var applicationName = context.Services.GetApplicationName();
        if (!applicationName.IsNullOrEmpty())
        {
            Configure<AbpSecurityLogOptions>(options =>
            {
                options.ApplicationName = applicationName!;
            });
        }

        var configuration = context.Services.GetConfiguration();
        context.Services.Configure<AbpStringEncryptionOptions>(options =>
        {
            var keySize = configuration["StringEncryption:KeySize"];
            if (!keySize.IsNullOrWhiteSpace())
            {
                if (int.TryParse(keySize, out var intValue))
                {
                    options.Keysize = intValue;
                }
            }

            var defaultPassPhrase = configuration["StringEncryption:DefaultPassPhrase"];
            if (!defaultPassPhrase.IsNullOrWhiteSpace())
            {
                options.DefaultPassPhrase = defaultPassPhrase!;
            }

            var initVectorBytes = configuration["StringEncryption:InitVectorBytes"];
            if (!initVectorBytes.IsNullOrWhiteSpace())
            {
                options.InitVectorBytes = Encoding.ASCII.GetBytes(initVectorBytes!);
            }

            var defaultSalt = configuration["StringEncryption:DefaultSalt"];
            if (!defaultSalt.IsNullOrWhiteSpace())
            {
                options.DefaultSalt = Encoding.ASCII.GetBytes(defaultSalt!);
            }
        });
    }


    private static void AutoAddClaimsPrincipalContributors(IServiceCollection services)
    {
        var contributorTypes = new List<Type>();
        var dynamicContributorTypes = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IAbpClaimsPrincipalContributor).IsAssignableFrom(context.ImplementationType))
            {
                contributorTypes.Add(context.ImplementationType);
            }

            if (typeof(IAbpDynamicClaimsPrincipalContributor).IsAssignableFrom(context.ImplementationType))
            {
                dynamicContributorTypes.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.Contributors.AddIfNotContains(contributorTypes);
            options.DynamicContributors.AddIfNotContains(dynamicContributorTypes);
        });
    }
}
