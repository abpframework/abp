using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.TextTemplating.Razor
{
    [Dependency(ReplaceServices = true)]
    public class RazorTemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAbpCompiledViewProvider _abpCompiledViewProvider;
        private readonly ITemplateDefinitionManager _templateDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public RazorTemplateRenderer(
            IServiceScopeFactory serviceScopeFactory,
            IAbpCompiledViewProvider abpCompiledViewProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _templateDefinitionManager = templateDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
            _abpCompiledViewProvider = abpCompiledViewProvider;
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
                    null,
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
                        null,
                        globalContext,
                        model
                    );
                }
            }
        }

        protected virtual async Task<string> RenderInternalAsync(
            string templateName,
            string body,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var templateDefinition = _templateDefinitionManager.Get(templateName);

            var renderedContent = await RenderSingleTemplateAsync(
                templateDefinition,
                body,
                globalContext,
                model
            );

            if (templateDefinition.Layout != null)
            {
                renderedContent = await RenderInternalAsync(
                    templateDefinition.Layout,
                    renderedContent,
                    globalContext
                );
            }

            return renderedContent;
        }

        protected virtual async Task<string> RenderSingleTemplateAsync(
            TemplateDefinition templateDefinition,
            string body,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            return await RenderTemplateContentWithRazorAsync(
                templateDefinition,
                body,
                globalContext,
                model
            );
        }

        protected virtual async Task<string> RenderTemplateContentWithRazorAsync(
            TemplateDefinition templateDefinition,
            string body,
            Dictionary<string, object> globalContext,
            object model = null)
        {
            var assembly = await _abpCompiledViewProvider.GetAssemblyAsync(templateDefinition);
            var templateType = assembly.GetType(AbpRazorTemplateConsts.TypeName);
            var template = (IRazorTemplatePage) Activator.CreateInstance(templateType);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var modelType = templateType
                    .GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRazorTemplatePage<>))
                    .Select(x => x.GenericTypeArguments.FirstOrDefault())
                    .FirstOrDefault();

                if (modelType != null)
                {
                    GetType().GetMethod(nameof(SetModel), BindingFlags.Instance | BindingFlags.NonPublic)
                        ?.MakeGenericMethod(modelType).Invoke(this, new[] {template, model});
                }

                template.ServiceProvider = scope.ServiceProvider;
                template.Localizer = GetLocalizerOrNull(templateDefinition);
                template.HtmlEncoder = scope.ServiceProvider.GetService<HtmlEncoder>();
                template.JavaScriptEncoder = scope.ServiceProvider.GetService<JavaScriptEncoder>();
                template.UrlEncoder = scope.ServiceProvider.GetService<UrlEncoder>();
                template.Body = body;
                template.GlobalContext = globalContext;

                await template.ExecuteAsync();

                return await template.GetOutputAsync();
            }
        }

        private void SetModel<TModel>(IRazorTemplatePage razorTemplatePage, object model = null)
        {
            if (razorTemplatePage is IRazorTemplatePage<TModel> razorTemplatePageWithModel)
            {
                razorTemplatePageWithModel.Model = (TModel)model;
            }
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
