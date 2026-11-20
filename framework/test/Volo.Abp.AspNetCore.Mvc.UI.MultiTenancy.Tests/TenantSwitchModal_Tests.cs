using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Pages.Abp.MultiTenancy;
using Shouldly;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;

public class TenantSwitchModal_Tests
{
    [Fact]
    public async Task Should_Reject_Long_Names_Before_Normalizing_Or_Resolving_The_Tenant()
    {
        var store = Substitute.For<ITenantStore>();
        var normalizer = Substitute.For<ITenantNormalizer>();
        using var services = new ServiceCollection()
            .AddSingleton<IModelStateValidator, ModelStateValidator>()
            .BuildServiceProvider();
        var model = new TenantSwitchModalModel(store, normalizer, Microsoft.Extensions.Options.Options.Create(new AbpAspNetCoreMultiTenancyOptions()))
        {
            Input = new TenantSwitchModalModel.TenantInfoModel { Name = new string('a', 257) },
            LazyServiceProvider = new AbpLazyServiceProvider(services),
            PageContext = new PageContext { HttpContext = new DefaultHttpContext() }
        };
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model.Input, new ValidationContext(model.Input), results, true).ShouldBeFalse();
        foreach (var result in results)
        {
            model.ModelState.AddModelError("Input.Name", result.ErrorMessage);
        }

        await Should.ThrowAsync<AbpValidationException>(() => model.OnPostAsync());

        normalizer.DidNotReceiveWithAnyArgs().NormalizeName(default);
        store.ReceivedCalls().ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("acme")]
    public void Should_Accept_Empty_And_Ordinary_Names(string name)
    {
        var input = new TenantSwitchModalModel.TenantInfoModel { Name = name };

        Validator.TryValidateObject(input, new ValidationContext(input), new List<ValidationResult>(), true).ShouldBeTrue();
    }

    [Fact]
    public void Should_Accept_A_Name_At_The_Length_Limit()
    {
        var input = new TenantSwitchModalModel.TenantInfoModel { Name = new string('a', 256) };

        Validator.TryValidateObject(input, new ValidationContext(input), new List<ValidationResult>(), true).ShouldBeTrue();
    }
}
