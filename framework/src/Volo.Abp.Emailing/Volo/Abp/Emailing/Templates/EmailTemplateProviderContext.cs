using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateProviderContext : IServiceProviderAccessor
    {
        public string Name { get; }

        public IServiceProvider ServiceProvider { get; }

        public EmailTemplate Template { get; set; }

        public EmailTemplateProviderContext(string name, IServiceProvider serviceProvider)
        {
            Name = name;
            ServiceProvider = serviceProvider;
        }
    }
}