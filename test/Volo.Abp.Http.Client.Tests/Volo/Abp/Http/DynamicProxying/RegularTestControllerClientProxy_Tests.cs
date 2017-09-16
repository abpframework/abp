using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class RegularTestControllerClientProxy_Tests : AbpHttpTestBase
    {
        //TODO: Create a regular MVC Controller and add different parameter bindings, verbs and routes and test client proxy for it!

        private readonly IRegularTestController _controller;

        public RegularTestControllerClientProxy_Tests()
        {
            _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        }

        [Fact]
        public void IncrementValue()
        {
            _controller.IncrementValue(42).ShouldBe(43);
        }
    }
}