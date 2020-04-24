using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFolderLocalizedTemplateContentReader : ILocalizedTemplateContentReader
    {
        private Dictionary<string, string> _dictionary;

        public async Task ReadContentsAsync(
            IVirtualFileProvider virtualFileProvider, 
            string virtualPath)
        {
            _dictionary = new Dictionary<string, string>();

            var directoryInfo = virtualFileProvider.GetFileInfo(virtualPath);
            if (!directoryInfo.IsDirectory)
            {
                throw new AbpException("Given virtual path is not a folder: " + virtualPath);
            }

            foreach (var file in virtualFileProvider.GetDirectoryContents(virtualPath))
            {
                if (file.IsDirectory)
                {
                    continue;
                }

                _dictionary.Add(file.Name.RemovePostFix(".tpl"), await file.ReadAsStringAsync());
            }
        }

        public string GetContent(string cultureName, string defaultCultureName)
        {
            var content = _dictionary.GetOrDefault(cultureName);
            if (content != null)
            {
                return content;
            }

            if (cultureName.Contains("-"))
            {
                var baseCultureName = CultureHelper.GetBaseCultureName(cultureName);
                content = _dictionary.GetOrDefault(baseCultureName);
                if (content != null)
                {
                    return content;
                }
            }

            if (defaultCultureName != null)
            {
                content = _dictionary.GetOrDefault(defaultCultureName);
                if (content != null)
                {
                    return content;
                }
            }

            return null;
        }
    }
}