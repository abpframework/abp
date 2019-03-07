using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Files;

namespace Volo.Blogging
{
    [RemoteService]
    [Area("blogging")]
    [Route("api/blogging/files")]
    public class BlogFilesController : AbpController, IFileAppService
    {
        private readonly IFileAppService _fileAppService;

        public BlogFilesController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost]
        public Task<FileUploadOutputDto> UploadAsync(FileUploadInputDto input)
        {
            return _fileAppService.UploadAsync(input);
        }
    }
}
