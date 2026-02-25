using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shouldly;
using Xunit;

namespace Volo.Abp.ObjectMapping;

public class ObjectMappingHelper_Tests
{
    [Fact]
    public void IsCollectionGenericType_Should_Return_True_For_Standard_GenericCollection()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<List<MappingTestSource>, List<MappingTestDestination>>(
            out var sourceArg, out var destArg, out var defGenericType);

        result.ShouldBeTrue();
        sourceArg.ShouldBe(typeof(MappingTestSource));
        destArg.ShouldBe(typeof(MappingTestDestination));
        defGenericType.ShouldBe(typeof(List<>));
    }

    [Fact]
    public void IsCollectionGenericType_Should_Return_True_For_Array()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<MappingTestSource[], MappingTestDestination[]>(
            out var sourceArg, out var destArg, out _);

        result.ShouldBeTrue();
        sourceArg.ShouldBe(typeof(MappingTestSource));
        destArg.ShouldBe(typeof(MappingTestDestination));
    }

    [Fact]
    public void IsCollectionGenericType_Should_Normalize_IEnumerable_To_List()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<IEnumerable<MappingTestSource>, IEnumerable<MappingTestDestination>>(
            out _, out _, out var defGenericType);

        result.ShouldBeTrue();
        defGenericType.ShouldBe(typeof(List<>));
    }

    [Fact]
    public void IsCollectionGenericType_Should_Normalize_ICollection_To_Collection()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<ICollection<MappingTestSource>, ICollection<MappingTestDestination>>(
            out _, out _, out var defGenericType);

        result.ShouldBeTrue();
        defGenericType.ShouldBe(typeof(Collection<>));
    }

    [Fact]
    public void IsCollectionGenericType_Should_Return_False_For_NonCollection()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<MappingTestSource, MappingTestDestination>(
            out _, out _, out _);

        result.ShouldBeFalse();
    }

    [Fact]
    public void IsCollectionGenericType_Should_Return_False_For_NonGeneric_DerivedCollection()
    {
        var result = ObjectMappingHelper.IsCollectionGenericType<List<MappingTestSource>, MappingTestDestinationList>(
            out _, out _, out _);

        result.ShouldBeFalse();
    }
}

public class MappingTestSource
{
    public string Value { get; set; } = "";
}

public class MappingTestDestination
{
    public string Value { get; set; } = "";
}

public class MappingTestDestinationList : List<MappingTestDestination>
{
}
