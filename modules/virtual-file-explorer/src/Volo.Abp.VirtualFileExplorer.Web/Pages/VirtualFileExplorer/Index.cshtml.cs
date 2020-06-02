using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using NUglify.Helpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.VirtualFileExplorer.Web.Models;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.VirtualFileSystem.Embedded;

namespace Volo.Abp.VirtualFileExplorer.Web.Pages.VirtualFileExplorer
{
    public class IndexModel : VirtualFileExplorerPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Path { get; set; } = "/";

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public List<FileInfoViewModel> FileInfoList { get; set; }

        public PagerModel PagerModel { get; set; }

        public string PathNavigation { get; set; }

        protected IVirtualFileProvider VirtualFileProvider { get; }


        public IndexModel(IVirtualFileProvider virtualFileProvider)
        {
            VirtualFileProvider = virtualFileProvider;
        }

        public virtual IActionResult OnGet()
        {
            var query = VirtualFileProvider.GetDirectoryContents(Path)
                .Where(d => VirtualFileExplorerConsts.AllowFileInfoTypes.Contains(d.GetType().Name))
                .OrderByDescending(f => f.IsDirectory).ToList();

            PagerModel = new PagerModel(query.Count, PageSize, CurrentPage, PageSize, $"{Url.Content("~/")}VirtualFileExplorer?Path={Path}&PageSize={PageSize}");

            SetViewModel(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
            SetPathNavigation();

            return Page();
        }

        private void SetViewModel(IEnumerable<IFileInfo> fileInfos)
        {
            FileInfoList = new List<FileInfoViewModel>();

            foreach (var fileInfo in fileInfos)
            {
                var fileInfoViewModel = new FileInfoViewModel()
                {
                    IsDirectory = fileInfo.IsDirectory,
                    Icon = "fas fa-file",
                    FileType = "file",
                    Length = fileInfo.Length+" bytes",
                    FileName = fileInfo.Name,
                    LastUpdateTime = fileInfo.LastModified.LocalDateTime
                };

                if (fileInfo.IsDirectory)
                {
                    fileInfoViewModel.Icon = "fas fa-folder";
                    fileInfoViewModel.FileType = "folder";
                    fileInfoViewModel.Length = "/";
                    fileInfoViewModel.FileName =$"<a href='{Url.Content("~/")}VirtualFileExplorer?path={fileInfo.PhysicalPath}'>{fileInfo.Name}</a>";
                }
                else
                {
                    fileInfoViewModel.FilePath = ((EmbeddedResourceFileInfo) fileInfo).VirtualPath;
                }

                FileInfoList.Add(fileInfoViewModel);
            }
        }

        private void SetPathNavigation()
        {
            var navigationBuild = new StringBuilder();
            var pathArray = Path.Split('/').Where(p => !p.IsNullOrWhiteSpace());
            var href = $"{Url.Content("~/")}VirtualFileExplorer?path=";

            navigationBuild.Append($"<nav aria-label='breadcrumb'>" +
                                   $" <ol class='breadcrumb'>" +
                                   $"<li class='breadcrumb-item'><a href='{href}/'>{L["BackToRoot"]}</a></li>");

            foreach (var item in pathArray)
            {
                href += "/" + item;
                navigationBuild.Append($"<li class='breadcrumb-item'><a href='{href}'>{item}</a></li>");
            }

            navigationBuild.Append("</ol></nav>");

            PathNavigation = navigationBuild.ToString();
        }
    }
}
