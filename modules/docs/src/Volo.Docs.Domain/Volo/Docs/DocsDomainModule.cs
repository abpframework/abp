using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Localization;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule)
        )]
    public class DocsDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<DocsDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<DocsDomainMappingProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<Document, DocumentEto>(typeof(DocsDomainModule));
                options.EtoMappings.Add<Project, ProjectEto>(typeof(DocsDomainModule));
            });
            
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

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                if (scope.ServiceProvider.GetRequiredService<IOptions<DocsElasticSearchOptions>>().Value.Enable)
                {
                    var documentFullSearch = scope.ServiceProvider.GetRequiredService<IDocumentFullSearch>();
                    AsyncHelper.RunSync(() => documentFullSearch.CreateIndexIfNeededAsync());
                }
            }
        }
    }
}
