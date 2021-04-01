using System.Collections.Generic;
using Volo.Abp.Content;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class CreateMultipleFileInput
    {
        public string Name { get; set; }

        public IEnumerable<IRemoteStreamContent> Contents { get; set; }

        public CreateFileInput Inner { get; set; }
    }
}
