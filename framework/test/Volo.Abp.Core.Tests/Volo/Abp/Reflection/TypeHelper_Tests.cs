using System;
using System.Collections.Generic;
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

    public class MyDictionary : Dictionary<bool, TypeHelper_Tests>
    {

    }

    public enum MyEnum
    {
        EnumValue0,
        EnumValue1,
    }
}
