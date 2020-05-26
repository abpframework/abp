using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring
{
    public class AbpBlobStoringOptions_Tests : AbpBlobStoringTestBase
    {
        private readonly AbpBlobStoringOptions _options;

        public AbpBlobStoringOptions_Tests()
        {
            _options = GetRequiredService<IOptions<AbpBlobStoringOptions>>().Value;
        }

        [Fact]
        public void Should_Property_Set_And_Get_Options_For_Different_Containers()
        {
            var testContainer1Config = _options.Containers
                .GetOrDefaultConfiguration(BlobContainerNameAttribute.GetContainerName<TestContainer1>());
            testContainer1Config.ShouldContainKeyAndValue("TestConfig1", "TestValue1");
            
            var testContainer2Config = _options.Containers
                .GetOrDefaultConfiguration(BlobContainerNameAttribute.GetContainerName<TestContainer2>());
            testContainer2Config.ShouldContainKeyAndValue("TestConfig2", "TestValue2");
        }
    }
}