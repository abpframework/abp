using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.Blogging.Files
{
    public interface IFileAppService : IApplicationService
    {
        Task<RawFileDto> GetAsync(string name);

        Task<IRemoteStreamContent> GetFileAsync(string name);

        Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input);
    }
}
