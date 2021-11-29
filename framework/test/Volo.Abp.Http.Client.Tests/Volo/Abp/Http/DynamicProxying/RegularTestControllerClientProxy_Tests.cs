using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class RegularTestControllerClientProxy_Tests : AbpHttpClientTestBase
    {
        //TODO: Create a regular MVC Controller and add different parameter bindings, verbs and routes and test client proxy for it!

        private readonly IRegularTestController _controller;

        public RegularTestControllerClientProxy_Tests()
        {
            _controller = ServiceProvider.GetRequiredService<IRegularTestController>();
        }

        [Fact]
        public async Task IncrementValueAsync()
        {
            (await _controller.IncrementValueAsync(42)).ShouldBe(43);
        }

        [Fact]
        public async Task GetException1Async()
        {
            var exception = await Assert.ThrowsAsync<AbpRemoteCallException>(async () => await _controller.GetException1Async());
            exception.Error.Message.ShouldBe("This is an error message!");
        }

        [Fact]
        public async Task GetException2Async()
        {
            var exception = await Assert.ThrowsAsync<AbpRemoteCallException>(async () => await _controller.GetException2Async());
            exception.Error.Message.ShouldBe("Business exception with data: TEST");
        }

        [Fact]
        public async Task GetWithDateTimeParameterAsync()
        {
            var dateTime1 = new DateTime(2020, 04, 19, 19, 05, 01);
            var result = await _controller.GetWithDateTimeParameterAsync(dateTime1);
            result.ToUniversalTime().ShouldBe(dateTime1.ToUniversalTime());
        }

        [Fact]
        public async Task GetWithDateTimeParameterAsync_With_Different_Culture()
        {
            using (CultureHelper.Use("es"))
            {
                var dateTime1 = new DateTime(2020, 04, 19, 19, 05, 01);
                var result = await _controller.GetWithDateTimeParameterAsync(dateTime1);
                result.ToUniversalTime().ShouldBe(dateTime1.ToUniversalTime());
            }
        }

        [Fact]
        public async Task PostValueWithHeaderAndQueryStringAsync()
        {
            var result = await _controller.PostValueWithHeaderAndQueryStringAsync("myheader", "myqs");
            result.ShouldBe("myheader#myqs");
        }

        [Fact]
        public async Task PostValueWithBodyAsync()
        {
            var result = await _controller.PostValueWithBodyAsync("mybody");
            result.ShouldBe("mybody");
        }

        [Fact]
        public async Task PutValueWithBodyAsync()
        {
            var result = await _controller.PutValueWithBodyAsync("mybody");
            result.ShouldBe("mybody");
        }

        [Fact]
        public async Task PostObjectWithBodyAsync()
        {
            var result = await _controller.PostObjectWithBodyAsync(new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
            result.Year.ShouldBe(1976);
            result.Model.ShouldBe("Ford");
        }

        [Fact]
        public async Task PostObjectWithQueryAsync()
        {
            var result = await _controller.PostObjectWithQueryAsync(new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
            result.Year.ShouldBe(1976);
            result.Model.ShouldBe("Ford");
        }

        [Fact]
        public async Task PostObjectWithQueryAsync_With_Different_Culture()
        {
            using (CultureHelper.Use("tr"))
            {
                var result = await _controller.PostObjectWithQueryAsync(new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
                result.Year.ShouldBe(1976);
                result.Model.ShouldBe("Ford");
            }
        }

        [Fact]
        public async Task GetObjectWithUrlAsync()
        {
            var result = await _controller.GetObjectWithUrlAsync(new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
            result.Year.ShouldBe(1976);
            result.Model.ShouldBe("Ford");
        }

        [Fact]
        public async Task GetObjectandIdAsync()
        {
            var result = await _controller.GetObjectandIdAsync(42, new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
            result.Year.ShouldBe(42);
            result.Model.ShouldBe("Ford");
        }

        [Fact]
        public async Task GetObjectAndIdWithQueryAsync()
        {
            var result = await _controller.GetObjectAndIdWithQueryAsync(42, new Car { Year = 1976, Model = "Ford", FirstReleaseDate = new DateTime(1976, 02, 22, 15, 0, 6, 22) });
            result.Year.ShouldBe(42);
            result.Model.ShouldBe("Ford");
        }

        [Fact]
        public async Task PatchValueWithBodyAsync()
        {
            var result = await _controller.PatchValueWithBodyAsync("mybody");
            result.ShouldBe("mybody");
        }

        [Fact]
        public async Task PutValueWithHeaderAndQueryStringAsync()
        {
            var result = await _controller.PutValueWithHeaderAndQueryStringAsync("myheader", "myqs");
            result.ShouldBe("myheader#myqs");
        }

        [Fact]
        public async Task PatchValueWithHeaderAndQueryStringAsync()
        {
            var result = await _controller.PatchValueWithHeaderAndQueryStringAsync("myheader", "myqs");
            result.ShouldBe("myheader#myqs");
        }

        [Fact]
        public async Task DeleteByIdAsync()
        {
            (await _controller.DeleteByIdAsync(42)).ShouldBe(43);
        }

        [Fact]
        public async Task AbortRequestAsync()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(10);

            var result = await _controller.AbortRequestAsync(default);
            result.ShouldBe("AbortRequestAsync");

            var exception = await Assert.ThrowsAsync<AbpRemoteCallException>(async () => await _controller.AbortRequestAsync(cts.Token));
            exception.InnerException.InnerException.InnerException.Message.ShouldBe("The client aborted the request.");
        }
    }
}
