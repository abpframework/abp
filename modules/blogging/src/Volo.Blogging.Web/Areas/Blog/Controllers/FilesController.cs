using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Areas.Blog.Models;
using Volo.Blogging.Hosting;

namespace Volo.Blogging.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("Blog/[controller]/[action]")]
    public class FilesController : AbpController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<JsonResult> UploadImage(IFormFile file)
        {
            file.ValidateImage(out var fileBytes);

            var fileUrl = await _fileService.SaveFileAsync(fileBytes, file.FileName);

            return Json(new FileUploadResult(fileUrl));
        }
    }
}