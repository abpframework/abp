using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations;

public class AbpValidationAttributeAdapterProvider_Tests
{
    private readonly AbpValidationAttributeAdapterProvider _provider = new(new ValidationAttributeAdapterProvider());

    [Fact]
    public void Should_Return_An_Adapter_For_The_EnumDataTypeAttribute()
    {
        //ASP.NET Core does not provide an adapter for the EnumDataTypeAttribute.
        new ValidationAttributeAdapterProvider()
            .GetAttributeAdapter(new EnumDataTypeAttribute(typeof(MyEnum)), null)
            .ShouldBeNull();

        _provider.GetAttributeAdapter(new EnumDataTypeAttribute(typeof(MyEnum)), null)
            .ShouldBeOfType<EnumDataTypeAttributeAdapter>();
    }

    [Fact]
    public void Should_Localize_The_Error_Message_Of_The_EnumDataTypeAttribute()
    {
        var attribute = new EnumDataTypeAttribute(typeof(MyEnum)) { ErrorMessage = "MyEnumIsInvalid" };

        var adapter = _provider.GetAttributeAdapter(attribute, new TestStringLocalizer())!;

        adapter.GetErrorMessage(CreateValidationContext()).ShouldBe("Localized:MyEnumIsInvalid");
    }

    private static ModelValidationContextBase CreateValidationContext()
    {
        var metadataProvider = new EmptyModelMetadataProvider();

        return new ClientModelValidationContext(
            new ActionContext(),
            metadataProvider.GetMetadataForProperty(typeof(MyModel), nameof(MyModel.Value)),
            metadataProvider,
            new Dictionary<string, string>()
        );
    }

    public enum MyEnum
    {
        Value1 = 1
    }

    public class MyModel
    {
        public MyEnum Value { get; set; }
    }

    private class TestStringLocalizer : IStringLocalizer
    {
        public LocalizedString this[string name] => new(name, "Localized:" + name);

        public LocalizedString this[string name, params object[] arguments] => new(name, "Localized:" + name);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return new List<LocalizedString>();
        }
    }
}
