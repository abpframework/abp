using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Blogs;

public class BlogFeatureDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly BlogFeatureManager _blogFeatureManager;
    private readonly IBlogRepository _blogRepository;

    public BlogFeatureDataSeedContributor(
        BlogFeatureManager blogFeatureManager,
        IBlogRepository blogRepository)
    {
        _blogFeatureManager = blogFeatureManager;
        _blogRepository = blogRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var blogs = await _blogRepository.GetListAsync();

        foreach (var blog in blogs)
        {
            await _blogFeatureManager.SetDefaultsIfNotSetAsync(blog.Id);
        }
    }
}
