// This file is part of DocumentsAdminClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Docs.Admin.Documents;

// ReSharper disable once CheckNamespace
namespace Volo.Docs.Admin.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDocumentAdminAppService), typeof(DocumentsAdminClientProxy))]
    public partial class DocumentsAdminClientProxy : ClientProxyBase<IDocumentAdminAppService>, IDocumentAdminAppService
    {
    }
}
