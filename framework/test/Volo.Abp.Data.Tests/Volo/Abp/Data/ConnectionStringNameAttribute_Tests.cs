using Shouldly;
using Xunit;

namespace Volo.Abp.Data;

public class ConnectionStringNameAttribute_Tests
{
    [Fact]
    public void Should_Get_Class_FullName_If_Not_ConnStringNameAttribute_Specified()
    {
        ConnectionStringNameAttribute
            .GetConnStringName<MyClassWithoutConnStringName>()
            .ShouldBe(typeof(MyClassWithoutConnStringName).FullName);
    }

    [Fact]
    public void Should_Get_ConnStringName_If_Not_Specified()
    {
        ConnectionStringNameAttribute
            .GetConnStringName<MyClassWithConnStringName>()
            .ShouldBe("MyDb");
    }
    private class MyClassWithoutConnStringName
    {

    }

    [ConnectionStringName("MyDb")]
    private class MyClassWithConnStringName
    {

    }
}
