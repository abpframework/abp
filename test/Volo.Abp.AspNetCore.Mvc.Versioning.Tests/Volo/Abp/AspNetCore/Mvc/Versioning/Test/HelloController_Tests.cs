using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Versioning.App;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.Test
{
    public class HelloController_Tests: AspNetCoreMvcVersioningTestBase
    {
        private readonly IHelloController _todoAppService;

        public HelloController_Tests()
        {
            _todoAppService = ServiceProvider.GetRequiredService<IHelloController>();
        }

        [Fact]
        public async Task PostAsync()
        {
            (await _todoAppService.PostAsync()).ShouldBe("42-2.0");
        }
    }
}
