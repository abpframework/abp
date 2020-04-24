using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContentContributor : ITemplateContentContributor, ITransientDependency
    {
        public const string VirtualPathPropertyName = "VirtualPath";

        private readonly IVirtualFileProvider _virtualFileProvider;

        public VirtualFileTemplateContentContributor(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
        }

        public async Task<string> GetOrNullAsync(TemplateContentContributorContext context)
        {
            var virtualPath = context.TemplateDefinition.Properties.GetOrDefault(VirtualPathPropertyName) as string;
            if (virtualPath == null)
            {
                return null;
            }

            //TODO: Refactor: Split implementation based on single file or dictionary of culture-specific contents

            var cultureName = context.Culture ??
                              CultureInfo.CurrentUICulture.Name;

            var dictionary = GetTemplateDictionary(virtualPath);

            var content = dictionary.GetOrDefault(cultureName);
            if (content != null)
            {
                return content;
            }

            if (cultureName.Contains("-"))
            {
                var baseCultureName = CultureHelper.GetBaseCultureName(cultureName);
                content = dictionary.GetOrDefault(baseCultureName);
                if (content != null)
                {
                    return content;
                }
            }

            if (context.TemplateDefinition.DefaultCultureName != null)
            {
                content = dictionary.GetOrDefault(context.TemplateDefinition.DefaultCultureName);
                if (content != null)
                {
                    return content;
                }
            }

            return dictionary.GetOrDefault("__default");
        }

        private Dictionary<string, string> GetTemplateDictionary(string virtualPath)
        {
            var dictionary = new Dictionary<string, string>();

            var fileInfo = _virtualFileProvider.GetFileInfo(virtualPath);
            if (!fileInfo.IsDirectory)
            {
                //TODO: __default to consts
                dictionary.Add("__default", fileInfo.ReadAsString());
            }
            else
            {
                foreach (var file in _virtualFileProvider.GetDirectoryContents(virtualPath))
                {
                    if (file.IsDirectory)
                    {
                        continue;
                    }

                    // TODO: How to normalize file names?
                    dictionary.Add(file.Name.RemovePostFix(".tpl"), file.ReadAsString());
                }
            }

            return dictionary;
        }
    }
}