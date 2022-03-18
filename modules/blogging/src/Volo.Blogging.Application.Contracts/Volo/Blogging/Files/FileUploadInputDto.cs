using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace Volo.Blogging.Files
{
    public class FileUploadInputDto
    {
        [Required]
        public IRemoteStreamContent File { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
