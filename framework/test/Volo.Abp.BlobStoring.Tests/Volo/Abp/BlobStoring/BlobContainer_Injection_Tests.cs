using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainer_Injection_Tests : AbpBlobStoringTestBase
    {
        [Fact]
        public void Test()
        {
            GetRequiredService<IBlobContainer<TestContainer1>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer1>>();
            
            GetRequiredService<IBlobContainer<TestContainer2>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer2>>();
            
            GetRequiredService<IBlobContainer<TestContainer3>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer3>>();
        }
    }
}