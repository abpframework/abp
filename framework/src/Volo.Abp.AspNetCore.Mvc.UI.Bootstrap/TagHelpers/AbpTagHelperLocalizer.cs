using System;
using System.Collections.Generic;
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
            var localizer = GetLocalizer(explorer);

            return localizer == null ? text : localizer[text].Value;
        }

        public IStringLocalizer GetLocalizer(ModelExplorer explorer)
        {
            return GetLocalizer(explorer);
        }

        private IStringLocalizer GetStringLocalizer(ModelExplorer explorer)
        {
            IStringLocalizer localizer = null;
            var resourceType = _options.AssemblyResources.GetOrDefault(explorer.Container.ModelType.Assembly);

            if (resourceType != null)
            {
                localizer = _stringLocalizerFactory.Create(resourceType);
            }

            return localizer;
        }
    }
}
