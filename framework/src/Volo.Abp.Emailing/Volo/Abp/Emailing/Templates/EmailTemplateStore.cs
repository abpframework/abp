using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateStore : IEmailTemplateStore, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected EmailTemplateOptions Options { get; }

        public EmailTemplateStore(IOptions<EmailTemplateOptions> options, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public async Task<EmailTemplate> GetAsync(string name)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new EmailTemplateProviderContext(name, scope.ServiceProvider);
                foreach (var provider in Options.Providers)
                {
                    await provider.ProvideAsync(context);
                }

                if (context.Template == null)
                {
                    //TODO: Return a default email template!
                    throw new NotImplementedException();
                }

                return context.Template;
            }
        }
    }
}