using Shouldly;
using Xunit;

namespace Volo.Abp.Data
{
    public class DatabaseNameAttribute_Tests
    {
        [Fact]
        public void Should_Get_Class_FullName_If_Not_DatabaseNameAttribute_Specified()
        {
            DatabaseNameAttribute
                .GetDatabaseName<MyClassWithoutDatabaseName>()
                .ShouldBe(typeof(MyClassWithoutDatabaseName).FullName);
        }

        [Fact]
        public void Should_Get_DatabaseName_If_Not_Specified()
        {
            DatabaseNameAttribute
                .GetDatabaseName<MyClassWithDatabaseName>()
                .ShouldBe("MyDb");
        }
        private class MyClassWithoutDatabaseName
        {
            
        }

        [DatabaseName("MyDb")]
        private class MyClassWithDatabaseName
        {

        }
    }
}
