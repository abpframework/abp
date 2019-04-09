using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;
using Volo.Blogging.Areas.Blog.Models;
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

        [HttpGet]
        [Route("{name}")]
        public Task<RawFileDto> GetAsync(string name) //TODO: output cache would be good
        {
            return _fileAppService.GetAsync(name);
        }

        [HttpGet]
        [Route("www/{name}")]
        public async Task<FileResult> GetForWebAsync(string name) //TODO: output cache would be good
        {
            var file = await _fileAppService.GetAsync(name);
            return File(
                file.Bytes,
                MimeTypes.GetByExtension(Path.GetExtension(name))
            );
        }

        [HttpPost]
        public Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            return _fileAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("images/upload")]
        public async Task<JsonResult> UploadImage(IFormFile file)
        {
            //TODO: localize exception messages

            if (file == null)
            {
                throw new UserFriendlyException("No file found!");
            }

            if (file.Length <= 0)
            {
                throw new UserFriendlyException("File is empty!");
            }

            if (!file.ContentType.Contains("image"))
            {
                throw new UserFriendlyException("Not a valid image!");
            }

            var output = await _fileAppService.CreateAsync(
                new FileUploadInputDto
                {
                    Bytes = file.GetAllBytes(),
                    Name = file.FileName
                }
            );

            return Json(new FileUploadResult(output.WebUrl));
        }
    }
}
