using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Xunit;

namespace Volo.Abp.Json;

public class AbpSystemTextJsonUnsupportedTypeMatcher_Tests : AbpJsonTestBase
{
    private readonly AbpSystemTextJsonUnsupportedTypeMatcher _abpSystemTextJsonUnsupportedTypeMatcher;

    public AbpSystemTextJsonUnsupportedTypeMatcher_Tests()
    {
        _abpSystemTextJsonUnsupportedTypeMatcher = GetRequiredService<AbpSystemTextJsonUnsupportedTypeMatcher>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.Add<MyClass>();
            options.UnsupportedTypes.Add<byte[]>();
            options.UnsupportedTypes.Add<Dictionary<string, MyClass4>>();
        });
    }

    [Fact]
    public void Match_Test()
    {
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(MyClass)).ShouldBeTrue();
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(byte[])).ShouldBeTrue();

        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(MyClass2)).ShouldBeFalse();
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(MyClass3)).ShouldBeFalse();
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(MyClass4)).ShouldBeFalse();

        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(string)).ShouldBeFalse();
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(string[])).ShouldBeFalse();

        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(Dictionary<string, MyClass4>)).ShouldBeTrue();
        _abpSystemTextJsonUnsupportedTypeMatcher.Match(typeof(IDictionary<string, MyClass4>)).ShouldBeFalse();
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
