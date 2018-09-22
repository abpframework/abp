using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Formatting
{
    public class DocumentConverterFactory : IDocumentConverterFactory, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentConverterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDocumentConverter Create(string format)
        {
            switch (format.ToLowerInvariant())
            {
                case MarkdownDocumentConverter.Type:
                    return _serviceProvider.GetRequiredService<MarkdownDocumentConverter>();
                default:
                    throw new ApplicationException($"Undefined document formatting: {format}");
            }
        }
    }
}