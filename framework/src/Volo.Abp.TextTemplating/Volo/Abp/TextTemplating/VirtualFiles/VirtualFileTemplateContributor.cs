using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContributor : ITemplateContributor
    {
        public TemplateDefinition TemplateDefinition { get; private set; }

        private readonly string _virtualPath;
        private IVirtualFileProvider _virtualFileProvider;
        private volatile Dictionary<string, string> _templateDictionary;
        private readonly object _syncObj = new object();

        public VirtualFileTemplateContributor(
            [NotNull] string virtualPath)
        {
            _virtualPath = Check.NotNullOrWhiteSpace(virtualPath, nameof(virtualPath));
        }

        public void Initialize(TemplateContributorInitializationContext context)
        {
            _virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
            TemplateDefinition = context.TemplateDefinition;
        }

        public async Task<string> GetOrNullAsync([CanBeNull] string cultureName = null)
        {
            //TODO: Refactor: Split implementation based on single file or dictionary of culture-specific contents

            cultureName ??= CultureInfo.CurrentUICulture.Name;

            var dictionary = GetTemplateDictionary();

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

            if (TemplateDefinition.DefaultCultureName != null)
            {
                content = dictionary.GetOrDefault(TemplateDefinition.DefaultCultureName);
                if (content != null)
                {
                    return content;
                }
            }

            return dictionary.GetOrDefault("__default");
        }

        private Dictionary<string, string> GetTemplateDictionary()
        {
            if (_templateDictionary != null)
            {
                return _templateDictionary;
            }

            lock (_syncObj)
            {
                if (_templateDictionary != null)
                {
                    return _templateDictionary;
                }

                var dictionary = new Dictionary<string, string>();

                var fileInfo = _virtualFileProvider.GetFileInfo(_virtualPath);
                if (!fileInfo.IsDirectory)
                {
                    //TODO: __default to consts
                    dictionary.Add("__default", fileInfo.ReadAsString());
                }
                else
                {
                    foreach (var file in _virtualFileProvider.GetDirectoryContents(_virtualPath))
                    {
                        if (file.IsDirectory)
                        {
                            continue;
                        }

                        // TODO: How to normalize file names?
                        dictionary.Add(file.Name.RemovePostFix(".tpl"), file.ReadAsString());
                    }
                }

                _templateDictionary = dictionary;
                return dictionary;
            }
        }
    }
}