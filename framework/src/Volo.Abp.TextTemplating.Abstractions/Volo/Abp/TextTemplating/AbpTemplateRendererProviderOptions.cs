using System;
using System.Collections.Generic;

namespace Volo.Abp.TextTemplating
{
    public class AbpTemplateRendererProviderOptions
    {
        protected IDictionary<string, Type> Providers { get; }

        public AbpTemplateRendererProviderOptions()
        {
            Providers = new Dictionary<string, Type>();
        }

        public AbpTemplateRendererProviderOptions AddProvider<TProvider>(string templateRenderEngineName)
            where TProvider : ITemplateRendererProvider
        {
            if (Providers.ContainsKey(templateRenderEngineName))
            {
                Providers.RemoveAll(x => x.Key == templateRenderEngineName);
            }

            Providers[templateRenderEngineName] = typeof(TProvider);

            return this;
        }

        public Type GetProviderTypeOrNull(string templateRenderEngineName)
        {
            return templateRenderEngineName == null ? null : Providers.GetOrDefault(templateRenderEngineName);
        }
    }
}
