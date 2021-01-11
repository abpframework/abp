using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Runtime;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.TextTemplating
{
    public class TemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        private readonly ITemplateContentProvider _templateContentProvider;
        private readonly ITemplateDefinitionManager _templateDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _templateContentProvider = templateContentProvider;
            _templateDefinitionManager = templateDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public virtual async Task<string> RenderAsync(
            [NotNull] string templateName,
            [CanBeNull] object model = null,
            [CanBeNull] string cultureName = null,
            [CanBeNull] Dictionary<string, object> globalContext = null)
        {
            Check.NotNullOrWhiteSpace(templateName, nameof(templateName));

            if (globalContext == null)
            {
                globalContext = new Dictionary<string, object>();
            }

            if (cultureName == null)
            {
                return await RenderInternalAsync(
                    templateName,
                    globalContext,
                    model
                );
            }
            else
            {
                using (CultureHelper.Use(cultureName))
                {
                    return await RenderInternalAsync(
                        templateName,
                        globalContext,
                        model
                    );
                }
            }
        }

        protected virtual async Task<string> RenderInternalAsync(
            string templateName,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var templateDefinition = _templateDefinitionManager.Get(templateName);

            var renderedContent = await RenderSingleTemplateAsync(
                templateDefinition,
                globalContext,
                model
            );

            if (templateDefinition.Layout != null)
            {
                globalContext["content"] = renderedContent;
                renderedContent = await RenderInternalAsync(
                    templateDefinition.Layout,
                    globalContext
                );
            }

            return renderedContent;
        }

        protected virtual async Task<string> RenderSingleTemplateAsync(
            TemplateDefinition templateDefinition,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var rawTemplateContent = await _templateContentProvider
                .GetContentOrNullAsync(
                    templateDefinition
                );

            return await RenderTemplateContentWithScribanAsync(
                templateDefinition,
                rawTemplateContent,
                globalContext,
                model
            );
        }

        protected virtual async Task<string> RenderTemplateContentWithScribanAsync(
            TemplateDefinition templateDefinition,
            string templateContent,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var context = CreateScribanTemplateContext(
                templateDefinition,
                globalContext,
                model
            );

            return await Template
                    .Parse(templateContent)
                    .RenderAsync(context);
        }

        protected virtual TemplateContext CreateScribanTemplateContext(
            TemplateDefinition templateDefinition,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var context = new TemplateContext();

            var scriptObject = new ScriptObject();

            scriptObject.Import(globalContext);

            if (model != null)
            {
                scriptObject["model"] = model;
            }

            var localizer = GetLocalizerOrNull(templateDefinition);
            if (localizer != null)
            {
                scriptObject.SetValue("L", new TemplateLocalizer(localizer), true);
            }

            context.PushGlobal(scriptObject);
            context.PushCulture(System.Globalization.CultureInfo.CurrentCulture);

            return context;
        }

        private IStringLocalizer GetLocalizerOrNull(TemplateDefinition templateDefinition)
        {
            if (templateDefinition.LocalizationResource != null)
            {
                return _stringLocalizerFactory.Create(templateDefinition.LocalizationResource);
            }

            return _stringLocalizerFactory.CreateDefaultOrNull();
        }
    }
}
