using System;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.Json
{
    public class SystemTextJsonSupportTypes_Tests : AbpJsonTestBase
    {
        private readonly SystemTextJsonSupportTypes _systemTextJsonSupportTypes;

        public SystemTextJsonSupportTypes_Tests()
        {
            _systemTextJsonSupportTypes = GetRequiredService<SystemTextJsonSupportTypes>();
        }

        [Fact]
        public void Test()
        {
            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass)).ShouldBeFalse();
            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass2)).ShouldBeFalse();
            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass3)).ShouldBeFalse();
            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass4)).ShouldBeFalse();

            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass5)).ShouldBeTrue();
            _systemTextJsonSupportTypes.CanHandle(typeof(MyClass6)).ShouldBeTrue();
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
    }
}
