using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Versioning.App.v2;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.Test.v2
{
    public class TodoAppService_Tests : AspNetCoreMvcVersioningTestBase
    {
        private readonly ITodoAppService _todoAppService;

        public TodoAppService_Tests()
        {
            _todoAppService = ServiceProvider.GetRequiredService<ITodoAppService>();
        }

        [Fact]
        public async Task GetAsync()
        {
            (await _todoAppService.GetAsync(42)).ShouldBe("42-2.0");
        }
    }
}
