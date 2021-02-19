using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public class MediaDescriptorDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string MimeType { get; set; }

        public int Size { get; set; }
    }
}