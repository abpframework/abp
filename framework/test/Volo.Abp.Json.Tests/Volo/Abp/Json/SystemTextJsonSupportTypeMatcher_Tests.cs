using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.Json
{
    public class SystemTextJsonSupportTypeMatcher_Tests : AbpJsonTestBase
    {
        private readonly SystemTextJsonSupportTypeMatcher _systemTextJsonSupportTypeMatcher;

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<SystemTextJsonSupportTypeMatcherOptions>(options =>
            {
                options.UnsupportedTypes.Add<MyClass7>();
            });

            base.AfterAddApplication(services);
        }

        public SystemTextJsonSupportTypeMatcher_Tests()
        {
            _systemTextJsonSupportTypeMatcher = GetRequiredService<SystemTextJsonSupportTypeMatcher>();
        }

        [Fact]
        public void CanHandle_Test()
        {
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass2)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass3)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass4)).ShouldBeFalse();

            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass5)).ShouldBeTrue();
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass6)).ShouldBeTrue();

            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass7)).ShouldBeFalse();
        }

        [DisableDateTimeNormalization]
        class MyClass
        {
            public DateTime Prop1 { get; set; }
        }

        class MyClass2
        {
            [DisableDateTimeNormalization]
            public DateTime Prop1 { get; set; }
        }

        class MyClass3
        {
            public MyClass4 Prop1 { get; set; }
        }

        class MyClass4
        {
            [DisableDateTimeNormalization]
            public DateTime Prop1 { get; set; }
        }

        class MyClass5
        {
            public DateTime Prop1 { get; set; }

            public MyClass6 Prop2 { get; set; }
        }

        class MyClass6
        {
            public DateTime Prop1 { get; set; }
        }

        class MyClass7
        {
            public DateTime Prop1 { get; set; }
        }
    }
}
