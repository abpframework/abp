using System;
using System.Collections.Generic;
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
                options.UnsupportedTypes.Add<byte>();
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

            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass8)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(MyClass9)).ShouldBeFalse();

            _systemTextJsonSupportTypeMatcher.Match(typeof(string)).ShouldBeTrue();
            _systemTextJsonSupportTypeMatcher.Match(typeof(string[])).ShouldBeTrue();

            _systemTextJsonSupportTypeMatcher.Match(typeof(int)).ShouldBeTrue();

            _systemTextJsonSupportTypeMatcher.Match(typeof(byte)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(Dictionary<byte, byte>)).ShouldBeFalse();
            _systemTextJsonSupportTypeMatcher.Match(typeof(Dictionary<string, MyClass10>)).ShouldBeFalse();
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

        class MyClass8
        {
            public MyClass10[] Prop1 { get; set; }
        }

        class MyClass9
        {
            public Dictionary<string, MyClass10> Prop1 { get; set; }
        }

        class MyClass10
        {
            [DisableDateTimeNormalization]
            public DateTime Prop1 { get; set; }
        }
    }
}
