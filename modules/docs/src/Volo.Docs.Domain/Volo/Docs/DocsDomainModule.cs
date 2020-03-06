using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddDomainModule)
        )]
    public class DocsDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets
                    .AddEmbedded<DocsDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<DocsResource>()
                    .AddVirtualJson("/Volo/Docs/Localization/Domain");
            });

            Configure<DocumentSourceOptions>(options =>
            {
                options.Sources[GithubDocumentSource.Type] = typeof(GithubDocumentSource);
                options.Sources[FileSystemDocumentSource.Type] = typeof(FileSystemDocumentSource);
            });

            context.Services.AddHttpClient(GithubRepositoryManager.HttpClientName, client =>
            {
                client.Timeout = TimeSpan.FromMilliseconds(15000);
            });
        }
    }
}
