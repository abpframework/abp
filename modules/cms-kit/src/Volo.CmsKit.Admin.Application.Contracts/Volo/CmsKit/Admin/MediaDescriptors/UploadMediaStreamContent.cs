using System.ComponentModel.DataAnnotations;
using System.IO;
using Volo.Abp.Content;
using Volo.Abp.Validation;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public class UploadMediaStreamContent : RemoteStreamContent
    {
        [Required]
        [DynamicStringLength(typeof(MediaDescriptorConsts), nameof(MediaDescriptorConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(MediaDescriptorConsts), nameof(MediaDescriptorConsts.MaxMimeTypeLength))]
        public string MimeType { get; set; }

        public UploadMediaStreamContent(Stream stream) : base(stream)
        {
        }
    }
}