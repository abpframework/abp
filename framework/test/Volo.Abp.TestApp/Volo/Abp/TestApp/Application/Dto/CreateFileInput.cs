using Volo.Abp.Content;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class CreateFileInput
    {
        public string Name { get; set; }

        public RemoteStreamContent Content { get; set; }
    }
}
