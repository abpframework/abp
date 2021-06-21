using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.VirtualFileExplorer.Web.Pages.VirtualFileExplorer
{
    public class FileContentModal : PageModel
    {
        [Required]
        [BindProperty(SupportsGet = true)]
        public string FilePath { get; set; }

        public string Content { get; set; }

        protected IVirtualFileProvider VirtualFileProvider { get; }

        public FileContentModal(IVirtualFileProvider virtualFileProvider)
        {
            VirtualFileProvider = virtualFileProvider;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var fileInfo = VirtualFileProvider.GetFileInfo(FilePath);
            if (fileInfo == null || fileInfo.IsDirectory)
            {
                return NotFound();
            }

            Content = await fileInfo.ReadAsStringAsync();

            return Page();
        }
    }
}
