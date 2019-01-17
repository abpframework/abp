using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class AbpTagHelperLocalizer : IAbpTagHelperLocalizer
    {
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly AbpMvcDataAnnotationsLocalizationOptions _options;

        public AbpTagHelperLocalizer(IOptions<AbpMvcDataAnnotationsLocalizationOptions> options, IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _options = options.Value;
        }

        public string GetLocalizedText(string text, ModelExplorer explorer)
        {
            var localizer = GetStringLocalizer(explorer.Container.ModelType.Assembly);

            return localizer == null ? text : localizer[text].Value;
        }

        public IStringLocalizer GetLocalizer(ModelExplorer explorer)
        {
            return GetStringLocalizer(explorer.Container.ModelType.Assembly);
        }

        public IStringLocalizer GetLocalizer(Assembly assembly)
        {
            return GetStringLocalizer(assembly);
        }

        private IStringLocalizer GetStringLocalizer(Assembly assembly)
        {
            IStringLocalizer localizer = null;
            var resourceType = _options.AssemblyResources.GetOrDefault(assembly);

            if (resourceType != null)
            {
                localizer = _stringLocalizerFactory.Create(resourceType);
            }

            return localizer;
        }
    }
}
