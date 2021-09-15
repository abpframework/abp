// This file is part of DocsDocumentClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Docs.Documents;

// ReSharper disable once CheckNamespace
namespace Volo.Docs.Documents.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDocumentAppService), typeof(DocsDocumentClientProxy))]
    public partial class DocsDocumentClientProxy : ClientProxyBase<IDocumentAppService>, IDocumentAppService
    {
    }
}
