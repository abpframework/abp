using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Blogging.Files;

namespace Volo.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConsts.RemoteServiceName)]
    [Area("blogging")]
    [Route("api/blogging/files")]
    public class BlogFilesController : AbpController, IFileAppService
    {
        private readonly IFileAppService _fileAppService;

        public BlogFilesController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpGet]
        [Route("{name}")]
        public Task<RawFileDto> GetAsync(string name) //TODO: output cache would be good
        {
            return _fileAppService.GetAsync(name);
        }

        [HttpGet]
        [Route("www/{name}")]
        public async Task<IRemoteStreamContent> GetFileAsync(string name)
        {
            return await _fileAppService.GetFileAsync(name);
        }

        [HttpPost]
        [Route("images/upload")]
        public Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            return _fileAppService.CreateAsync(input);
        }
    }
}
