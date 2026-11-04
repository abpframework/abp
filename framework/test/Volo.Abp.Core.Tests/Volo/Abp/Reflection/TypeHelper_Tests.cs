using System;
using System.Collections.Generic;
using System.Globalization;
using Shouldly;
using Xunit;

namespace Volo.Abp.Reflection;

public class TypeHelper_Tests
{
    [Fact]
    public void IsNonNullablePrimitiveType()
    {
        TypeHelper.IsNonNullablePrimitiveType(typeof(int)).ShouldBeTrue();
        TypeHelper.IsNonNullablePrimitiveType(typeof(string)).ShouldBeFalse();
    }

    [Fact]
    public void Should_Generic_Type_From_Nullable()
    {
        var nullableType = typeof(Guid?);
        var guidType = nullableType.GetFirstGenericArgumentIfNullable();

        guidType.ShouldBe(typeof(Guid));
    }

    [Fact]
    public void IsEnumerable()
    {
        TypeHelper.IsEnumerable(
            typeof(IEnumerable<string>),
            out var itemType
        ).ShouldBeTrue();
        itemType.ShouldBe(typeof(string));

        TypeHelper.IsEnumerable(
            typeof(List<TypeHelper_Tests>),
            out itemType
        ).ShouldBeTrue();
        itemType.ShouldBe(typeof(TypeHelper_Tests));

        TypeHelper.IsEnumerable(
            typeof(TypeHelper_Tests),
            out itemType
        ).ShouldBeFalse();
    }

    [Fact]
    public void IsDictionary()
    {
        //Dictionary<string, int>
        TypeHelper.IsDictionary(
            typeof(Dictionary<string, int>),
            out var keyType,
            out var valueType
        ).ShouldBeTrue();
        keyType.ShouldBe(typeof(string));
        valueType.ShouldBe(typeof(int));

        //MyDictionary
        TypeHelper.IsDictionary(
            typeof(MyDictionary),
            out keyType,
            out valueType
        ).ShouldBeTrue();
        keyType.ShouldBe(typeof(bool));
        valueType.ShouldBe(typeof(TypeHelper_Tests));

        //TypeHelper_Tests
        TypeHelper.IsDictionary(
            typeof(TypeHelper_Tests),
            out keyType,
            out valueType
        ).ShouldBeFalse();
    }

    [Fact]
    public void GetDefaultValue()
    {
        TypeHelper.GetDefaultValue(typeof(bool)).ShouldBe(false);
        TypeHelper.GetDefaultValue(typeof(byte)).ShouldBe(0);
        TypeHelper.GetDefaultValue(typeof(int)).ShouldBe(0);
        TypeHelper.GetDefaultValue(typeof(string)).ShouldBeNull();
        TypeHelper.GetDefaultValue(typeof(MyEnum)).ShouldBe(MyEnum.EnumValue0);
    }

    [Fact]
    public void ConvertFromString()
    {
        TypeHelper.ConvertFromString<int>("42").ShouldBe(42);
        TypeHelper.ConvertFromString<int?>("42").ShouldBe((int?)42);
        TypeHelper.ConvertFromString<int?>(null).ShouldBeNull();
    }

    [Theory]
    [InlineData(0.1, "0.1")]
    [InlineData(19.99, "19.99")]
    [InlineData(-2878.8, "-2878.8")]
    [InlineData(123456.789012, "123456.789012")]
    public void ChangeTypePrimitiveExtended_Should_Keep_A_Double_As_Written(double value, string expected)
    {
        var amount = decimal.Parse(expected, CultureInfo.InvariantCulture);

        TypeHelper.ChangeTypePrimitiveExtended<decimal>(value).ShouldBe(amount);
        TypeHelper.ChangeTypePrimitiveExtended<decimal?>(value).ShouldBe(amount);
        value.To<decimal>().ShouldBe(amount);
    }

    [Theory]
    [InlineData(19.99f, "19.99")]
    [InlineData(1299.9f, "1299.9")]
    [InlineData(-0.01f, "-0.01")]
    public void ChangeTypePrimitiveExtended_Should_Keep_A_Float_As_Written(float value, string expected)
    {
        var amount = decimal.Parse(expected, CultureInfo.InvariantCulture);

        TypeHelper.ChangeTypePrimitiveExtended<decimal>(value).ShouldBe(amount);
        value.To<decimal>().ShouldBe(amount);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(1e29)]
    [InlineData(7.9228162514264338E+28)]
    public void ChangeTypePrimitiveExtended_Should_Refuse_A_Double_A_Decimal_Cannot_Hold(double value)
    {
        Should.Throw<OverflowException>(() => TypeHelper.ChangeTypePrimitiveExtended<decimal>(value));
    }

    [Fact]
    public void ChangeTypePrimitiveExtended_Should_Convert_Other_Numbers_As_Before()
    {
        TypeHelper.ChangeTypePrimitiveExtended<decimal>(42L).ShouldBe(42m);
        TypeHelper.ChangeTypePrimitiveExtended<decimal>("19.99").ShouldBe(19.99m);
        TypeHelper.ChangeTypePrimitiveExtended<double>(0.1m).ShouldBe(0.1d);
        TypeHelper.ChangeTypePrimitiveExtended<int>(2.0d).ShouldBe(2);
    }

    public class MyDictionary : Dictionary<bool, TypeHelper_Tests>
    {

    }

    public enum MyEnum
    {
        EnumValue0,
        EnumValue1,
    }
}
