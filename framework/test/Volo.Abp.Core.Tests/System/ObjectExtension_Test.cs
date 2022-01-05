using Shouldly;
using Xunit;

namespace System;

public class ObjectExtensions_Tests
{
    [Fact]
    public void As_Test()
    {
        var obj = (object)new ObjectExtensions_Tests();
        obj.As<ObjectExtensions_Tests>().ShouldNotBe(null);

        obj = null;
        obj.As<ObjectExtensions_Tests>().ShouldBe(null);
    }

    [Fact]
    public void To_Tests()
    {
        "42".To<int>().ShouldBeOfType<int>().ShouldBe(42);
        "42".To<Int32>().ShouldBeOfType<Int32>().ShouldBe(42);

        "28173829281734".To<long>().ShouldBeOfType<long>().ShouldBe(28173829281734);
        "28173829281734".To<Int64>().ShouldBeOfType<Int64>().ShouldBe(28173829281734);

        "2.0".To<double>().ShouldBe(2.0);
        "0.2".To<double>().ShouldBe(0.2);
        (2.0).To<int>().ShouldBe(2);

        "false".To<bool>().ShouldBeOfType<bool>().ShouldBe(false);
        "True".To<bool>().ShouldBeOfType<bool>().ShouldBe(true);
        "False".To<bool>().ShouldBeOfType<bool>().ShouldBe(false);
        "TrUE".To<bool>().ShouldBeOfType<bool>().ShouldBe(true);

        Assert.Throws<FormatException>(() => "test".To<bool>());
        Assert.Throws<FormatException>(() => "test".To<int>());

        "2260AFEC-BBFD-42D4-A91A-DCB11E09B17F".To<Guid>().ShouldBeOfType<Guid>().ShouldBe(new Guid("2260afec-bbfd-42d4-a91a-dcb11e09b17f"));
    }

    [Fact]
    public void IsIn_Test()
    {
        5.IsIn(1, 3, 5, 7).ShouldBe(true);
        6.IsIn(1, 3, 5, 7).ShouldBe(false);

        int? number = null;
        number.IsIn(2, 3, 5).ShouldBe(false);

        var str = "a";
        str.IsIn("a", "b", "c").ShouldBe(true);

        str = null;
        str.IsIn("a", "b", "c").ShouldBe(false);
    }

    [Fact]
    public void If_Tests()
    {
        var value = 0;

        value = value.If(true, v => v + 1);
        value.ShouldBe(1);

        value = value.If(false, v => v + 1);
        value.ShouldBe(1);

        value = value
            .If(true, v => v + 3)
            .If(false, v => v + 5);
        value.ShouldBe(4);
    }
}
