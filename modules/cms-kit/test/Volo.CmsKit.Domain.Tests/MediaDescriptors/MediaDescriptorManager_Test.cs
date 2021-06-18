using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Blogs;
using Xunit;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptorManager_Test : CmsKitDomainTestBase
    {
        private readonly MediaDescriptorManager manager;
        private readonly CmsKitTestData testData;

        public MediaDescriptorManager_Test()
        {
            manager = GetRequiredService<MediaDescriptorManager>();
            testData = GetRequiredService<CmsKitTestData>();
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkProperly_WithDefinedEntityType()
        {
            var created = await manager.CreateAsync(testData.Media_1_EntityType, "MyAwesomeImage.png", "image/png", 128000);

            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WithUndefinedEntityType()
        {
            var undefinedEntityType = "My.Any.EntityType";

            var exception = await Should.ThrowAsync<EntityCantHaveMediaException>(async () =>
                                await manager.CreateAsync(undefinedEntityType, "import.json", "application/json", 256000));

            exception.ShouldNotBeNull();
            exception.EntityType.ShouldBe(undefinedEntityType);
        }
    }
}
