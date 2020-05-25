using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerNameAttribute_Tests
    {
        [Fact]
        public void Should_Get_Specified_Name()
        {
            BlobContainerNameAttribute
                .GetContainerName<MyContainerType2>()
                .ShouldBe("ContName2");
        }
        
        [Fact]
        public void Should_Get_Full_Class_Name_If_Not_Specified()
        {
            BlobContainerNameAttribute
                .GetContainerName<MyContainerType1>()
                .ShouldBe(typeof(MyContainerType1).FullName);
        }
        
        private class MyContainerType1
        {
            
        }
        
        [BlobContainerName("ContName2")]
        private class MyContainerType2
        {
            
        }
    }
}