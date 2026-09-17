using System.Collections.Generic;
using Shouldly;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.ValueComparers;

public class AbpJsonValueComparer_Tests
{
    [Fact]
    public void Should_Compare_Collections_By_Content()
    {
        var comparer = new AbpJsonValueComparer<List<string>>();

        comparer.Equals(new List<string> { "a", "b" }, new List<string> { "a", "b" }).ShouldBeTrue();
        comparer.Equals(new List<string> { "a", "b" }, new List<string> { "b", "a" }).ShouldBeFalse();
        comparer.GetHashCode(new List<string> { "a", "b" })
            .ShouldBe(comparer.GetHashCode(new List<string> { "a", "b" }));
    }

    [Fact]
    public void Should_Handle_Null_Values()
    {
        var comparer = new AbpJsonValueComparer<List<string>>();

        comparer.Equals(null, null).ShouldBeTrue();
        comparer.Equals(new List<string>(), null).ShouldBeFalse();
    }

    [Fact]
    public void Snapshot_Should_Be_A_Deep_Copy()
    {
        var comparer = new AbpJsonValueComparer<List<string>>();
        var original = new List<string> { "a" };

        var snapshot = (List<string>)comparer.Snapshot(original)!;
        original.Add("b");

        snapshot.ShouldBe(new[] { "a" });
        comparer.Equals(original, snapshot).ShouldBeFalse();
    }

    [Fact]
    public void Snapshot_Should_Copy_Nested_Arrays_Of_Typed_Objects()
    {
        var comparer = new AbpJsonValueComparer<SampleData>();
        var original = new SampleData { Count = 1, Tags = new[] { "x" } };

        var snapshot = (SampleData)comparer.Snapshot(original)!;
        original.Tags[0] = "changed";
        original.Count = 2;

        snapshot.Tags.ShouldBe(new[] { "x" });
        snapshot.Count.ShouldBe(1);
        comparer.Equals(original, snapshot).ShouldBeFalse();
    }

    [Fact]
    public void Should_Support_Nullable_Value_Types()
    {
        var comparer = new AbpJsonValueComparer<int?>();

        comparer.Equals(42, 42).ShouldBeTrue();
        comparer.Equals(42, 43).ShouldBeFalse();
        comparer.Equals(null, null).ShouldBeTrue();
    }

    private class SampleData
    {
        public int Count { get; set; }

        public string[] Tags { get; set; } = default!;
    }
}
