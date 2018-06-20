using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Versioning.App;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.Test
{
    public class TodoAppService_Tests : AspNetCoreMvcVersioningTestBase
    {
        private readonly ITodoAppService _todoAppService;

        public TodoAppService_Tests()
        {
            _todoAppService = ServiceProvider.GetRequiredService<ITodoAppService>();
        }

        [Fact]
        public void Get()
        {
            _todoAppService.Get(42).ShouldBe("42-2.0");
        }
    }
}
