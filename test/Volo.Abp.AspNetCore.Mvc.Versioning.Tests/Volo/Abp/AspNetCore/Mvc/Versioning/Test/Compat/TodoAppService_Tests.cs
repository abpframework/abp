using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Versioning.App.Compat;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.Test.Compat
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
            _todoAppService.Get(42).ShouldBe("Compat-42-1.0");
        }
    }
}
