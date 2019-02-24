using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Blogging.Files
{
    public interface IFileAppService : IApplicationService
    {
        Task<FileUploadOutputDto> UploadAsync(FileUploadInputDto input);
    }
}
