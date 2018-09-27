using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Documents
{
    public class DocumentStoreFactory : IDocumentStoreFactory, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentStoreFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDocumentStore Create(string documentStoreType)
        {
            switch (documentStoreType)
            {
                case GithubDocumentStore.Type:
                    return _serviceProvider.GetRequiredService<GithubDocumentStore>();
                default:
                    throw new ApplicationException($"Undefined document store: {documentStoreType}");
            }
        }
    }
}