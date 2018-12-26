using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.HtmlConverting
{
    public class DocumentToHtmlConverterFactory : IDocumentToHtmlConverterFactory, ITransientDependency
    {
        protected DocumentToHtmlConverterOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public DocumentToHtmlConverterFactory(
            IServiceProvider serviceProvider,
            IOptions<DocumentToHtmlConverterOptions> options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public virtual IDocumentToHtmlConverter Create(string format)
        {
            var serviceType = Options.Converters.GetOrDefault(format);
            if (serviceType == null)
            {
                throw new ApplicationException($"Unknown document format: {format}");
            }

            return (IDocumentToHtmlConverter)ServiceProvider.GetRequiredService(serviceType);
        }
    }
}