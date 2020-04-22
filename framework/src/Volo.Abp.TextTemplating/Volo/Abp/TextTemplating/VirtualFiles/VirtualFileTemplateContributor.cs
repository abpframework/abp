using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContributor : ITemplateContributor
    {
        private readonly string _virtualPath;
        private IVirtualFileProvider _virtualFileProvider;
        private volatile Dictionary<string, string> _templateDictionary;
        private readonly object _syncObj = new object();

        public VirtualFileTemplateContributor(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public void Initialize(TemplateContributorInitializationContext context)
        {
            _virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
        }

        public string GetOrNull([CanBeNull] string cultureName = null)
        {
            var dictionary = GetTemplateDictionary();

            if (cultureName == null)
            {
                return dictionary.GetOrDefault("__default");
            }
            else
            {
                return dictionary.GetOrDefault(cultureName) ??
                       dictionary.GetOrDefault("__default");
            }
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