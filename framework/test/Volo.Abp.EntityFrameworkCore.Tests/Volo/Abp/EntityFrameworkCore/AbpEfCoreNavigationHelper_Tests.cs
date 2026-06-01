using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore;

public class AbpEfCoreNavigationHelper_Tests : EntityFrameworkCoreTestBase
{
    private readonly IRepository<Blog, Guid> _blogRepository;

    public AbpEfCoreNavigationHelper_Tests()
    {
        _blogRepository = GetRequiredService<IRepository<Blog, Guid>>();
    }

    [Fact]
    public async Task Performance_Test()
    {
        //These time taken varies on different machines.
        //I used relatively large values, but it can also check for performance problem.
        var batchUpdateTime = TimeSpan.FromSeconds(30);
        var queryTime = TimeSpan.FromSeconds(10);

        if (!Environment.GetEnvironmentVariable("GITHUB_ACTIONS").IsNullOrWhiteSpace())
        {
            batchUpdateTime = batchUpdateTime * 6;
            queryTime = queryTime * 6;
        }


        var stopWatch = Stopwatch.StartNew();
        await WithUnitOfWorkAsync(async () =>
        {
            for (var i = 0; i < 5 * 1000; i++)
            {
                await _blogRepository.InsertAsync(
                    new Blog(Guid.NewGuid())
                    {
                        Name = "Blog" + i,
                        BlogPosts =
                        [
                            new BlogPost(Guid.NewGuid())
                            {
                                Title = "Post" + i
                            }
                        ]
                    });
            }
        });
        stopWatch.Stop();
        stopWatch.Elapsed.ShouldBeLessThan(batchUpdateTime);


        stopWatch.Restart();
        var blogs = await _blogRepository.GetListAsync(includeDetails: true);
        blogs.Count.ShouldBe(5 * 1000);
        blogs.SelectMany(x => x.BlogPosts).Count().ShouldBe(5 * 1000);
        stopWatch.Stop();
        stopWatch.Elapsed.ShouldBeLessThan(queryTime);


        var blogId = blogs.First().Id;
        stopWatch.Restart();
        await WithUnitOfWorkAsync(async () =>
        {
            var blog = await _blogRepository.GetAsync(blogId);
            blog.ShouldNotBeNull();
            for (var i = 0; i < 5 * 1000; i++)
            {
                blog.BlogPosts.Add(
                    new BlogPost(Guid.NewGuid())
                    {
                        Title = "NewPost" + i
                    });
            }
            await _blogRepository.UpdateAsync(blog);
        });
        stopWatch.Stop();
        stopWatch.Elapsed.ShouldBeLessThan(batchUpdateTime);

        stopWatch.Restart();
        var blog = await _blogRepository.GetAsync(blogId);
        blog.BlogPosts.Count.ShouldBe(5 * 1000 + 1);
        stopWatch.Stop();
        stopWatch.Elapsed.ShouldBeLessThan(queryTime);
    }
}
