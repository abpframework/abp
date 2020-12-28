using Volo.Abp.Modularity;

namespace Volo.CmsKit.Tags
{
    public abstract class TagRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly ITagRepository _tagRepository;

        protected TagRepository_Test()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _tagRepository = GetRequiredService<ITagRepository>();
        }
    }
}