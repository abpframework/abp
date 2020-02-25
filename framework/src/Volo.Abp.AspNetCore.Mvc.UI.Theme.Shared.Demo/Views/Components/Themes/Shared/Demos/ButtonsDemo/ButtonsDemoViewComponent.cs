using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.ButtonsDemo
{
    [Widget]
    public class ButtonsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/ButtonsDemo/Default.cshtml";

        private const string DemoSectionOpeningTag = "<DemoSection:";
        private const string DemoSectionClosingTag = "</DemoSection:";

        private readonly IVirtualFileProvider _virtualFileProvider;

        public ButtonsDemoViewComponent(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
        }

        public IViewComponentResult Invoke()
        {
            var model = new DemoViewModel();

            var viewFileInfo = _virtualFileProvider.GetFileInfo(ViewPath);
            var viewFileContent = viewFileInfo.ReadAsString();

            var lines = viewFileContent.SplitToLines();

            StringBuilder sb = null;

            foreach (var line in lines)
            {
                if (line.Contains(DemoSectionOpeningTag))
                {
                    sb = new StringBuilder();
                }
                else if (line.Contains(DemoSectionClosingTag, StringComparison.InvariantCultureIgnoreCase))
                {
                    var demoName = line.Substring(line.IndexOf(DemoSectionClosingTag, StringComparison.InvariantCultureIgnoreCase) + DemoSectionClosingTag.Length);
                    if (demoName.Contains(">"))
                    {
                        demoName = demoName.Left(demoName.IndexOf(">", StringComparison.InvariantCultureIgnoreCase));
                    }

                    model.SourceCodes[demoName] = sb.ToString();
                    sb = new StringBuilder();
                }
                else if(sb != null)
                {
                    sb.AppendLine(line);
                }
            }

            return View(ViewPath, model);
        }
    }

    public class DemoViewModel
    {
        public Dictionary<string, string> SourceCodes { get; set; }

        public DemoViewModel()
        {
            SourceCodes = new Dictionary<string, string>();
        }
    }
}
