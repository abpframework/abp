using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Xunit;

namespace Volo.Abp.Json
{
    public class AbpSystemTextJsonSupportTypeMatcher_Tests : AbpJsonTestBase
    {
        private readonly AbpSystemTextJsonSupportTypeMatcher _abpSystemTextJsonSupportTypeMatcher;

        public AbpSystemTextJsonSupportTypeMatcher_Tests()
        {
            _abpSystemTextJsonSupportTypeMatcher = GetRequiredService<AbpSystemTextJsonSupportTypeMatcher>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpSystemTextJsonSupportTypeMatcherOptions>(options =>
            {
                options.UnsupportedTypes.Add<MyClass>();
                options.UnsupportedTypes.Add<byte[]>();
                options.UnsupportedTypes.Add<Dictionary<string, MyClass4>>();
            });
        }

        [Fact]
        public void CanHandle_Test()
        {
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(MyClass)).ShouldBeFalse();
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(byte[])).ShouldBeFalse();

            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(MyClass2)).ShouldBeTrue();
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(MyClass3)).ShouldBeTrue();
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(MyClass4)).ShouldBeTrue();

            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(string)).ShouldBeTrue();
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(string[])).ShouldBeTrue();

            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(Dictionary<string, MyClass4>)).ShouldBeFalse();
            _abpSystemTextJsonSupportTypeMatcher.Match(typeof(IDictionary<string, MyClass4>)).ShouldBeTrue();
        }

        class MyClass
        {
            public DateTime Prop1 { get; set; }
        }

        class MyClass2
        {
            public DateTime Prop1 { get; set; }
        }

        class MyClass3
        {
            public MyClass4 Prop1 { get; set; }
        }

        class MyClass4
        {
            public DateTime Prop1 { get; set; }
        }
    }
}
