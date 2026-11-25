using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Pages;
using Xunit;

namespace Volo.CmsKit;

public class PublicLookupCache_Tests : CmsKitApplicationTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.AddSingleton<LookupRecordingCache>();
        services.AddSingleton<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(
            provider => provider.GetRequiredService<LookupRecordingCache>());
    }

    [Fact]
    public async Task Missing_Pages_Should_Not_Create_Cache_Entries()
    {
        var pages = GetRequiredService<IPagePublicAppService>();
        var recorder = GetRequiredService<LookupRecordingCache>();
        recorder.Writes.Clear();

        for (var i = 0; i < 12; i++)
        {
            var slug = "missing-page-" + i;
            (await pages.FindBySlugAsync(slug)).ShouldBeNull();
            (await pages.DoesSlugExistAsync(slug)).ShouldBeFalse();
        }

        recorder.Writes.ShouldNotContain(key => key.Contains("CmsPage_"));
    }

    [Theory]
    [InlineData(PageStatus.Draft)]
    [InlineData(PageStatus.Publish)]
    public async Task Only_Published_Pages_Should_Be_Cached_At_The_Slug_Limit(PageStatus status)
    {
        var slug = new string('a', PageConsts.MaxSlugLength);
        await GetRequiredService<IPageAdminAppService>().CreateAsync(new CreatePageInputDto
        {
            Title = "Boundary page",
            Slug = slug,
            Status = status
        });
        var pages = GetRequiredService<IPagePublicAppService>();
        var cache = GetRequiredService<IDistributedCache<PageCacheItem>>();
        var recorder = GetRequiredService<LookupRecordingCache>();
        recorder.Writes.Clear();

        var result = await pages.FindBySlugAsync(slug);
        var cached = await cache.GetAsync(PageCacheItem.GetKey(slug));
        if (status == PageStatus.Publish)
        {
            result.ShouldNotBeNull();
            cached.ShouldNotBeNull();
            cached.Id.ShouldBe(result.Id);
            (await pages.DoesSlugExistAsync(slug)).ShouldBeTrue();
            recorder.Writes.Count(key => key.Contains("CmsPage_")).ShouldBe(1);
        }
        else
        {
            result.ShouldBeNull();
            cached.ShouldBeNull();
            recorder.Writes.ShouldNotContain(key => key.Contains("CmsPage_"));
        }
    }

    [Fact]
    public async Task Missing_Blog_Features_Should_Return_The_Default_Without_Caching()
    {
        var service = GetRequiredService<IBlogFeatureAppService>();
        var data = GetRequiredService<CmsKitTestData>();
        var recorder = GetRequiredService<LookupRecordingCache>();
        recorder.Writes.Clear();

        for (var i = 0; i < 12; i++)
        {
            foreach (var blogId in new[] { data.Blog_Id, Guid.NewGuid() })
            {
                var featureName = "MissingFeature" + i;
                var result = await service.GetOrDefaultAsync(blogId, featureName);
                result.Id.ShouldBe(Guid.Empty);
                result.FeatureName.ShouldBe(featureName);
                result.IsEnabled.ShouldBeTrue();
            }
        }

        recorder.Writes.ShouldNotContain(key => key.Contains("BlogFeature_"));
    }

    [Fact]
    public async Task Stored_Features_Should_Be_Cached_And_Invalidated_When_Updated()
    {
        var data = GetRequiredService<CmsKitTestData>();
        var service = GetRequiredService<IBlogFeatureAppService>();
        var cache = GetRequiredService<IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey>>();
        var key = new BlogFeatureCacheKey(data.Blog_Id, data.BlogFeature_2_FeatureName);

        (await service.GetOrDefaultAsync(data.Blog_Id, key.FeatureName)).IsEnabled.ShouldBeFalse();
        (await cache.GetAsync(key)).Id.ShouldBe(data.BlogFeature_2_Id);
        await WithUnitOfWorkAsync(() => GetRequiredService<BlogFeatureManager>().SetAsync(
            data.Blog_Id, key.FeatureName, true));
        (await cache.GetAsync(key)).ShouldBeNull();
        (await service.GetOrDefaultAsync(data.Blog_Id, key.FeatureName)).IsEnabled.ShouldBeTrue();
    }

}

public sealed class LookupRecordingCache : Microsoft.Extensions.Caching.Distributed.IDistributedCache
{
    private readonly MemoryDistributedCache _inner = new(Options.Create(new MemoryDistributedCacheOptions()));
    public ConcurrentQueue<string> Writes { get; } = new();
    public byte[] Get(string key) => _inner.Get(key);
    public Task<byte[]> GetAsync(string key, CancellationToken token = default) => _inner.GetAsync(key, token);
    public void Refresh(string key) => _inner.Refresh(key);
    public Task RefreshAsync(string key, CancellationToken token = default) => _inner.RefreshAsync(key, token);
    public void Remove(string key) => _inner.Remove(key);
    public Task RemoveAsync(string key, CancellationToken token = default) => _inner.RemoveAsync(key, token);
    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        Writes.Enqueue(key);
        _inner.Set(key, value, options);
    }
    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        Writes.Enqueue(key);
        return _inner.SetAsync(key, value, options, token);
    }
}
