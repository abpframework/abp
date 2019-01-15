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

        [Fact(Skip = "It stopped working after ASP.NET Core 2.2 Upgrade. Should work on that.")]
        public void Get()
        {
            _todoAppService.Get(42).ShouldBe("42-2.0");
        }
    }
}
