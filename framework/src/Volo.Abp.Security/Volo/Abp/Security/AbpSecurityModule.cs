using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.Security;

public class AbpSecurityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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
                options.DefaultPassPhrase = defaultPassPhrase;
            }

            var initVectorBytes = configuration["StringEncryption:InitVectorBytes"];
            if (!initVectorBytes.IsNullOrWhiteSpace())
            {
                options.InitVectorBytes = Encoding.ASCII.GetBytes(initVectorBytes); ;
            }

            var defaultSalt = configuration["StringEncryption:DefaultSalt"];
            if (!defaultSalt.IsNullOrWhiteSpace())
            {
                options.DefaultSalt = Encoding.ASCII.GetBytes(defaultSalt); ;
            }
        });
    }
}
