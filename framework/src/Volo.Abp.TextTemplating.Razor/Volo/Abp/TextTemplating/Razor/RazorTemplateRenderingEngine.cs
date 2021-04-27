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
    public class RazorTemplateRenderingEngine : TemplateRenderingEngineBase, ITransientDependency
    {
        public const string EngineName = "Razor";
        public override string Name => EngineName;

        protected readonly IServiceScopeFactory ServiceScopeFactory;
        protected readonly IAbpCompiledViewProvider AbpCompiledViewProvider;

        public RazorTemplateRenderingEngine(
            IServiceScopeFactory serviceScopeFactory,
            IAbpCompiledViewProvider abpCompiledViewProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            ITemplateContentProvider templateContentProvider,
            IStringLocalizerFactory stringLocalizerFactory)
            : base(templateDefinitionManager, templateContentProvider, stringLocalizerFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            AbpCompiledViewProvider = abpCompiledViewProvider;
        }

        public override async Task<string> RenderAsync(
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
            var templateDefinition = TemplateDefinitionManager.Get(templateName);

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
            var assembly = await AbpCompiledViewProvider.GetAssemblyAsync(templateDefinition);
            var templateType = assembly.GetType(AbpRazorTemplateConsts.TypeName);
            var template = (IRazorTemplatePage) Activator.CreateInstance(templateType);

            using (var scope = ServiceScopeFactory.CreateScope())
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
    }
}
