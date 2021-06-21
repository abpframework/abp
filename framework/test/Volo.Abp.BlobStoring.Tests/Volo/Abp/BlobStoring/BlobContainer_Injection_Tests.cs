using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainer_Injection_Tests : AbpBlobStoringTestBase
    {
        [Fact]
        public void Should_Inject_DefaultContainer_For_Non_Generic_Interface()
        {
            GetRequiredService<IBlobContainer>()
                .ShouldBeOfType<BlobContainer<DefaultContainer>>();
        }

        [Fact]
        public void Should_Inject_Specified_Container_For_Generic_Interface()
        {
            GetRequiredService<IBlobContainer<DefaultContainer>>()
                .ShouldBeOfType<BlobContainer<DefaultContainer>>();

            GetRequiredService<IBlobContainer<TestContainer1>>()
                .ShouldBeOfType<BlobContainer<TestContainer1>>();
            
            GetRequiredService<IBlobContainer<TestContainer2>>()
                .ShouldBeOfType<BlobContainer<TestContainer2>>();
            
            GetRequiredService<IBlobContainer<TestContainer3>>()
                .ShouldBeOfType<BlobContainer<TestContainer3>>();
        }
    }
}