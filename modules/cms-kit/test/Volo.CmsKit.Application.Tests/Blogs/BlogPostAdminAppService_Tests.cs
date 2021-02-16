using System;
using System.IO;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly IBlogPostAdminAppService blogPostAdminAppService;
        private readonly CmsKitTestData cmsKitTestData;
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostAdminAppService_Tests()
        {
            blogPostAdminAppService = GetRequiredService<IBlogPostAdminAppService>();
            cmsKitTestData = GetRequiredService<CmsKitTestData>();
            blogPostRepository = GetRequiredService<IBlogPostRepository>();
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkProperly_WithCorrectData()
        {
            var title = "My awesome new Post";
            var slug = "my-awesome-new-post";
            var shortDescription = "This blog is all about awesomeness 🤗!";

            var created = await blogPostAdminAppService.CreateAsync(new CreateBlogPostDto
            {
                BlogId = cmsKitTestData.Blog_Id,
                Title = title,
                Slug = slug,
                ShortDescription = shortDescription
            });

            created.Id.ShouldNotBe(Guid.Empty);

            var blogPost = await blogPostRepository.GetAsync(created.Id);

            blogPost.Title.ShouldBe(title);
            blogPost.Slug.ShouldBe(slug);
            blogPost.ShortDescription.ShouldBe(shortDescription);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WithNonExistingBlogId()
        {
            var title = "Another My Awesome New Post";
            var slug = "another-my-awesome-new-post";
            var shortDescription = "This blog is all about awesomeness 🤗!";

            var dto = new CreateBlogPostDto
            {
                // Non-existing Id
                BlogId = Guid.NewGuid(),
                Title = title,
                Slug = slug,
                ShortDescription = shortDescription
            };

            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAdminAppService.CreateAsync(dto));

            exception.EntityType.ShouldBe(typeof(Blog));
        }

        [Fact]
        public async Task GetAsync_ShouldWorkProperly_WithExistingId()
        {
            var blogPost = await blogPostAdminAppService.GetAsync(cmsKitTestData.BlogPost_1_Id);

            blogPost.Title.ShouldBe(cmsKitTestData.BlogPost_1_Title);
            blogPost.Slug.ShouldBe(cmsKitTestData.BlogPost_1_Slug);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowException_WithNonExistingId()
        {
            var nonExistingId = Guid.NewGuid();
            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAdminAppService.GetAsync(nonExistingId));

            exception.EntityType.ShouldBe(typeof(BlogPost));
            exception.Id.ShouldBe(nonExistingId);
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldWorkProperly_WithExistingSlug()
        {
            var blogPost = await blogPostAdminAppService.GetBySlugAsync(cmsKitTestData.BlogSlug, cmsKitTestData.BlogPost_1_Slug);

            blogPost.Id.ShouldBe(cmsKitTestData.BlogPost_1_Id);
            blogPost.Title.ShouldBe(cmsKitTestData.BlogPost_1_Title);
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldThrowException_WithNonExistingBlogPostSlug()
        {
            var nonExistingSlug = "any-other-url";
            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAdminAppService.GetBySlugAsync(cmsKitTestData.BlogSlug, nonExistingSlug));

            exception.EntityType.ShouldBe(typeof(BlogPost));
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldThrowException_WithNonExistingBlogSlug()
        {
            var nonExistingSlug = "any-other-url";
            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAdminAppService.GetBySlugAsync(nonExistingSlug, cmsKitTestData.Page_1_Url));

            exception.EntityType.ShouldBe(typeof(Blog));
        }

        [Fact]
        public async Task GetListAsync_ShouldWorkProperly_WithDefaultParameters()
        {
            var list = await blogPostAdminAppService.GetListAsync(new PagedAndSortedResultRequestDto());

            list.ShouldNotBeNull();
            list.TotalCount.ShouldBe(2);
            list.Items.ShouldNotBeEmpty();
            list.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkProperly_WithRegularDatas()
        {
            var shortDescription = "Another short description";
            var title = "[Solved] Another Blog Post";
            var slug = "another-short-blog-post";

            await blogPostAdminAppService.UpdateAsync(cmsKitTestData.BlogPost_2_Id, new UpdateBlogPostDto
            {
                ShortDescription = shortDescription,
                Title = title,
                Slug = slug,
            });

            var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_2_Id);

            blogPost.Title.ShouldBe(title);
            blogPost.ShortDescription.ShouldBe(shortDescription);
            blogPost.Slug.ShouldBe(slug);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhileUpdatingWithAlreadyExistingSlug()
        {
            var dto = new UpdateBlogPostDto
            {
                Title = "Some new title",
                Slug = cmsKitTestData.BlogPost_1_Slug
            };

            var exception = await Should.ThrowAsync<BlogPostSlugAlreadyExistException>(async () =>
                                await blogPostAdminAppService.UpdateAsync(cmsKitTestData.BlogPost_2_Id, dto));

            exception.Slug.ShouldBe(cmsKitTestData.BlogPost_1_Slug);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkProperly_WithExistingId()
        {
            await blogPostAdminAppService.DeleteAsync(cmsKitTestData.Page_2_Id);

            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostRepository.GetAsync(cmsKitTestData.Page_2_Id));

            exception.EntityType.ShouldBe(typeof(BlogPost));
            exception.Id.ShouldBe(cmsKitTestData.Page_2_Id);
        }

        [Fact]
        public async Task SetCoverImage_ShouldNotThrowException_WithCorrectData()
        {
            using (var imageStream = GetSampleImageStream())
            {
                await WithUnitOfWorkAsync(async () =>
                {
                    await blogPostAdminAppService.SetCoverImageAsync(
                            cmsKitTestData.BlogPost_2_Id,
                            new RemoteStreamContent(imageStream));
                });
            }
        }

        private Stream GetSampleImageStream()
        {
            var assembly = GetType().Assembly;
            var resourceName = "Volo.CmsKit.Data.BlogPostSample.png";

            return assembly.GetManifestResourceStream(resourceName);
        }
        // SetCoverImage
    }
}
