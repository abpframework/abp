using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Formatting
{
    public class DocumentFormattingFactory : IDocumentFormattingFactory, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentFormattingFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDocumentFormatting Create(string format)
        {
            switch (format.ToLowerInvariant())
            {
                case MarkdownDocumentFormatting.Type:
                    return _serviceProvider.GetRequiredService<MarkdownDocumentFormatting>();
                default:
                    throw new ApplicationException($"Undefined document formatting: {format}");
            }
        }
    }
}