using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Documents
{
    public class DocumentStoreFactory : IDocumentStoreFactory, ITransientDependency
    {
        protected DocumentStoreOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public DocumentStoreFactory(
            IServiceProvider serviceProvider,
            IOptions<DocumentStoreOptions> options)
        {
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public virtual IDocumentStore Create(string storeType)
        {
            var serviceType = Options.Stores.GetOrDefault(storeType);
            if (serviceType == null)
            {
                throw new ApplicationException($"Undefined document store: {storeType}");
            }

            return (IDocumentStore) ServiceProvider.GetRequiredService(serviceType);
        }
    }
}