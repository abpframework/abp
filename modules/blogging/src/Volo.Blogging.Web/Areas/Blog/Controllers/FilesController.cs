using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Areas.Blog.Models;
using Volo.Blogging.Files;
using Volo.Blogging.Hosting;

namespace Volo.Blogging.Areas.Blog.Controllers
{
    //TODO: This may be moved to HttpApi project since it may be needed by a SPA too.
    [Area("Blog")]
    [Route("Blog/[controller]/[action]")]
    public class FilesController : AbpController
    {
        private readonly IFileAppService _fileAppService;

        public FilesController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost]
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

            var output = await _fileAppService.UploadAsync(
                new FileUploadInputDto
                {
                    Bytes = file.AsBytes(),
                    Name = file.FileName
                }
            );

            return Json(new FileUploadResult(output.Url));
        }
    }
}