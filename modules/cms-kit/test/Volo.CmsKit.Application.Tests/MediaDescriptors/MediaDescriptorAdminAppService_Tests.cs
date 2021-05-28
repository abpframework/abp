using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Content;
using Volo.CmsKit.Admin.MediaDescriptors;
using Xunit;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptorAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IMediaDescriptorAdminAppService _mediaDescriptorAdminAppService;
        private readonly IMediaDescriptorRepository _mediaDescriptorRepository;

        public MediaDescriptorAdminAppService_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _mediaDescriptorAdminAppService = GetRequiredService<IMediaDescriptorAdminAppService>();
            _mediaDescriptorRepository = GetRequiredService<IMediaDescriptorRepository>();
        }

        [Fact]
        public async Task Should_Create_Media()
        {
            var mediaName = "README.md";
            var mediaType = "text/markdown";
            var mediaContent =
                "# ABP Framework\nABP Framework is a complete **infrastructure** based on the **ASP.NET Core** to create **modern web applications** and **APIs** by following the software development **best practices** and the **latest technologies**.";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(mediaContent));

            var media = await _mediaDescriptorAdminAppService.CreateAsync(_cmsKitTestData.Media_1_EntityType, new CreateMediaInputWithStream
            {
                Name = mediaName,
                File = new RemoteStreamContent(stream, mediaType)
            });
            
            media.ShouldNotBeNull();
        }
        
        [Fact]
        public async Task Should_Delete_Media()
        {
            await _mediaDescriptorAdminAppService.DeleteAsync(_cmsKitTestData.Media_1_Id);

            (await _mediaDescriptorRepository.FindAsync(_cmsKitTestData.Media_1_Id)).ShouldBeNull();
        }
    }
}