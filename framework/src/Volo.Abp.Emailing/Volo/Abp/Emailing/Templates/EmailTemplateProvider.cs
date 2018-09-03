using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected ITemplateLocalizer TemplateLocalizer { get; }
        protected EmailTemplateOptions Options { get; }

        public EmailTemplateProvider(
            IOptions<EmailTemplateOptions> options, 
            IServiceProvider serviceProvider,
            ITemplateLocalizer templateLocalizer)
        {
            ServiceProvider = serviceProvider;
            TemplateLocalizer = templateLocalizer;
            Options = options.Value;
        }

        public async Task<EmailTemplate> GetAsync(string name)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                return await GetInternalAsync(scope.ServiceProvider, name);
            }
        }

        protected virtual async Task<EmailTemplate> GetInternalAsync(IServiceProvider serviceProvider, string name)
        {
            var context = new EmailTemplateProviderContributorContext(name, serviceProvider);

            foreach (var provider in Options.Providers)
            {
                await provider.ProvideAsync(context);
            }

            if (context.Template == null)
            {
                throw new AbpException($"Could not found the template: {name}");
            }

            await SetLayoutAsync(serviceProvider, context);
            await LocalizeAsync(serviceProvider, context);
            
            return context.Template;
        }

        protected virtual async Task SetLayoutAsync(IServiceProvider serviceProvider, EmailTemplateProviderContributorContext context)
        {
            var layout = context.Template.Definition.Layout;

            if (layout.IsNullOrEmpty())
            {
                return;
            }
            
            if (layout == EmailTemplateDefinition.DefaultLayoutPlaceHolder)
            {
                layout = Options.DefaultLayout;
            }

            var layoutTemplate = await GetInternalAsync(serviceProvider, layout);
            context.Template.SetLayout(layoutTemplate);
        }

        protected virtual Task LocalizeAsync(IServiceProvider serviceProvider, EmailTemplateProviderContributorContext context)
        {
            if (context.Template.Definition.LocalizationResource == null)
            {
                return Task.CompletedTask;
            }

            var localizer = serviceProvider
                .GetRequiredService<IStringLocalizerFactory>()
                .Create(context.Template.Definition.LocalizationResource);

            context.Template.SetContent(
                TemplateLocalizer.Localize(localizer, context.Template.Content)
            );

            return Task.CompletedTask;
        }
    }
}