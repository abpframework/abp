using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.FluentValidation.TestObjects;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Http.FluentValidation;

public class FluentValidationApiDescription_WithoutAutofac_Tests
    : AbpHttpFluentValidationTestBase<FluentValidationApiDescription_WithoutAutofac_Tests.TestModule>
{
    [DependsOn(typeof(AbpHttpFluentValidationModule))]
    public class TestModule : AbpModule
    {
    }

    [Fact]
    public async Task Should_Contribute_Without_Autofac()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.LengthValue));

        property.MinLength.ShouldBe(3);
        property.MaxLength.ShouldBe(10);
    }
}
