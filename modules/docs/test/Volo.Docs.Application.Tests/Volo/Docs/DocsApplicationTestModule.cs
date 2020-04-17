using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationModule),
        typeof(DocsDomainTestModule)
        )]
    public class DocsApplicationTestModule : AbpModule
    {

    }
}
