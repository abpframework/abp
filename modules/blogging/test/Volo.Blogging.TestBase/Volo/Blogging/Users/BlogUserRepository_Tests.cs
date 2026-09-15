using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Blogging.Users
{
    public abstract class BlogUserRepository_Tests<TStartupModule> : BloggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IBlogUserRepository BlogUserRepository { get; }

        protected BlogUserRepository_Tests()
        {
            BlogUserRepository = GetRequiredService<IBlogUserRepository>();
        }

        [Fact]
        public async Task GetUsersAsync_Should_Sort_By_UserName_Before_Taking()
        {
            foreach (var userName in new[] { "sort-test-c", "sort-test-a", "sort-test-b" })
            {
                await BlogUserRepository.InsertAsync(new BlogUser(new UserData
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = userName + "@abp.io"
                }), autoSave: true);
            }

            var users = await BlogUserRepository.GetUsersAsync(2, "sort-test-");

            users.Select(x => x.UserName).ShouldBe(new[] { "sort-test-a", "sort-test-b" });
        }
    }
}
