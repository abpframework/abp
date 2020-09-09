using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public class AbpRequestLocalizationOptionsFactory : AbpOptionsFactory<RequestLocalizationOptions>
    {
        private readonly IAbpRequestLocalizationOptionsProvider _abpRequestLocalizationOptionsProvider;

        public AbpRequestLocalizationOptionsFactory(
            IAbpRequestLocalizationOptionsProvider abpRequestLocalizationOptionsProvider,
            IEnumerable<IConfigureOptions<RequestLocalizationOptions>> setups, 
            IEnumerable<IPostConfigureOptions<RequestLocalizationOptions>> postConfigures) 
            : base(
                setups, 
                postConfigures)
        {
            _abpRequestLocalizationOptionsProvider = abpRequestLocalizationOptionsProvider;
        }

        public override RequestLocalizationOptions Create(string name)
        {
            return _abpRequestLocalizationOptionsProvider.GetLocalizationOptions();
        }
    }
}