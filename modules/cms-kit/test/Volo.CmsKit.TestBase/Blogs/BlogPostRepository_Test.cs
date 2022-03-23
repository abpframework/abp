using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Blogs;

public abstract class BlogPostRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData testData;
    private readonly IBlogPostRepository blogPostRepository;

    public BlogPostRepository_Test()
    {
        testData = GetRequiredService<CmsKitTestData>();
        blogPostRepository = GetRequiredService<IBlogPostRepository>();
    }

    [Fact]
    public async Task SlugExistsAsync_ShouldReturnTrue_WithExistingSlug()
    {
        var result = await blogPostRepository.SlugExistsAsync(testData.Blog_Id, testData.BlogPost_1_Slug);

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task SlugExistsAsync_ShouldReturnFalse_WithNonExistingSlug()
    {
        var nonExistingSlug = "any-other-url-slug";

        var result = await blogPostRepository.SlugExistsAsync(testData.Blog_Id, nonExistingSlug);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task SlugExistsAsync_ShouldReturnFalse_WithNonExistingBlogId()
    {
        var nonExistingBlogId = Guid.NewGuid();

        var result = await blogPostRepository.SlugExistsAsync(nonExistingBlogId, testData.BlogPost_1_Slug);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldWorkProperly_WithCorrectParameters()
    {
        var blogPost = await blogPostRepository.GetBySlugAsync(testData.Blog_Id, testData.BlogPost_1_Slug);

        blogPost.ShouldNotBeNull();
        blogPost.Id.ShouldBe(testData.BlogPost_1_Id);
        blogPost.Slug.ShouldBe(testData.BlogPost_1_Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldHaveAuthor_WithCorrectParameters()
    {
        var blogPost = await blogPostRepository.GetBySlugAsync(testData.Blog_Id, testData.BlogPost_1_Slug);

        blogPost.ShouldNotBeNull();
        blogPost.Id.ShouldBe(testData.BlogPost_1_Id);
        blogPost.Slug.ShouldBe(testData.BlogPost_1_Slug);
        blogPost.Author.ShouldNotBeNull();
        blogPost.Author.Id.ShouldBe(testData.User1Id);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldThrowException_WithNonExistingBlogPostSlug()
    {
        var nonExistingSlugUrl = "absolutely-non-existing-url";
        var exception = await Should.ThrowAsync<EntityNotFoundException>(
            async () => await blogPostRepository.GetBySlugAsync(testData.Blog_Id, nonExistingSlugUrl));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(typeof(BlogPost));
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldThrowException_WithNonExistingBlogId()
    {
        var nonExistingBlogId = Guid.NewGuid();
        var exception = await Should.ThrowAsync<EntityNotFoundException>(
            async () => await blogPostRepository.GetBySlugAsync(nonExistingBlogId, testData.BlogPost_1_Slug));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(typeof(BlogPost));
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting10_WithoutSorting()
    {
        var result = await blogPostRepository.GetListAsync(null, testData.Blog_Id);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldHaveAuthor_WithBlogId_WhileGetting10_WithoutSorting()
    {
        var result = await blogPostRepository.GetListAsync(null, testData.Blog_Id);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);

        result.ForEach(blogPost => blogPost.Author.ShouldNotBeNull());
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting1_WithoutSorting()
    {
        var result = await blogPostRepository.GetListAsync(blogId: testData.Blog_Id, maxResultCount: 1);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting1InPage2_WithoutSorting()
    {
        var result = await blogPostRepository.GetListAsync(blogId: testData.Blog_Id, skipCount: 1, maxResultCount: 1);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting10_WithSortingByTitle()
    {
        var result = await blogPostRepository.GetListAsync(blogId: testData.Blog_Id, sorting: $"{nameof(BlogPost.Title)} asc");

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetAuthorsHasBlogPosts_ShouldWorkProperly()
    {
        var authors = await blogPostRepository.GetAuthorsHasBlogPosts();

        authors.ShouldNotBeNull();
        authors.ShouldNotBeEmpty();
        authors.ShouldContain(x => x.Id == testData.User1Id);
    }
}
