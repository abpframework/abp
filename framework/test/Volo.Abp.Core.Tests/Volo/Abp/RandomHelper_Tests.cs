using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp;

public class RandomHelper_Tests
{
    [Fact]
    public void GetRandom_Should_Return_Value_In_The_Given_Range()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = RandomHelper.GetRandom(10, 20);
            value.ShouldBeGreaterThanOrEqualTo(10);
            value.ShouldBeLessThan(20);
        }
    }

    [Fact]
    public void GetRandom_Should_Handle_Empty_Range()
    {
        RandomHelper.GetRandom(0).ShouldBe(0);
        RandomHelper.GetRandom(5, 5).ShouldBe(5);
    }

    [Fact]
    public void GetRandom_Should_Throw_Exception_For_Invalid_Range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomHelper.GetRandom(-1)).ParamName.ShouldBe("maxValue");
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomHelper.GetRandom(10, 5)).ParamName.ShouldBe("minValue");
    }

    [Fact]
    public void GenerateRandomizedList_Should_Keep_All_Items()
    {
        var items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        var randomizedList = RandomHelper.GenerateRandomizedList(items);

        randomizedList.OrderBy(x => x).ShouldBe(items);
    }

    [Fact]
    public void GenerateRandomizedList_Should_Keep_Duplicate_Items()
    {
        var items = new List<int> { 1, 1, 2, 2, 2, 3 };

        var randomizedList = RandomHelper.GenerateRandomizedList(items);

        randomizedList.OrderBy(x => x).ShouldBe(items);
    }

    [Fact]
    public void GenerateRandomizedList_Should_Work_With_Empty_Items()
    {
        RandomHelper.GenerateRandomizedList(new List<int>()).ShouldBeEmpty();
    }

    [Fact]
    public void GenerateRandomizedList_Should_Work_With_Single_Item()
    {
        RandomHelper.GenerateRandomizedList(new List<int> { 42 }).ShouldBe(new List<int> { 42 });
    }

    [Fact]
    public void GenerateRandomizedList_Should_Not_Change_The_Given_Items()
    {
        var items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        RandomHelper.GenerateRandomizedList(items);

        items.ShouldBe(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 });
    }

    [Fact]
    public void GenerateRandomizedList_Should_Enumerate_The_Given_Items_Once()
    {
        var enumerationCount = 0;

        IEnumerable<int> GetItems()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
            yield return 3;
        }

        RandomHelper.GenerateRandomizedList(GetItems());

        enumerationCount.ShouldBe(1);
    }

    [Fact]
    public void GenerateRandomizedList_Should_Throw_Exception_For_Null_Items()
    {
        Assert
            .Throws<ArgumentNullException>(() => RandomHelper.GenerateRandomizedList<int>(null!))
            .ParamName.ShouldBe("items");
    }
}
