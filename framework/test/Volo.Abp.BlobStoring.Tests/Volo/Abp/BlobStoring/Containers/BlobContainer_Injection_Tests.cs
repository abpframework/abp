using Shouldly;
using Volo.Abp.BlobStoring.Containers.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring.Containers
{
    public class BlobContainer_Injection_Tests : AbpBlobStoringTestBase
    {
        [Fact]
        public void Should_Inject_DefaultContainer_For_Non_Generic_Interface()
        {
            GetRequiredService<IBlobContainer>()
                .ShouldBeOfType<TypedBlobContainerWrapper<DefaultContainer>>();
        }

        [Fact]
        public void Should_Inject_Specified_Container_For_Generic_Interface()
        {
            GetRequiredService<IBlobContainer<DefaultContainer>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<DefaultContainer>>();

            GetRequiredService<IBlobContainer<TestContainer1>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer1>>();
            
            GetRequiredService<IBlobContainer<TestContainer2>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer2>>();
            
            GetRequiredService<IBlobContainer<TestContainer3>>()
                .ShouldBeOfType<TypedBlobContainerWrapper<TestContainer3>>();
        }
    }
}