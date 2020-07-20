using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileProvider : IVirtualFileProvider, ISingletonDependency
    {
        private readonly IFileProvider _hybridFileProvider;
        private readonly AbpVirtualFileSystemOptions _options;

        public VirtualFileProvider(
            IOptions<AbpVirtualFileSystemOptions> options,
            IDynamicFileProvider dynamicFileProvider)
        {
            _options = options.Value;
            _hybridFileProvider = CreateHybridProvider(dynamicFileProvider);
        }

        public virtual IFileInfo GetFileInfo(string subpath)
        {
            return _hybridFileProvider.GetFileInfo(subpath);
        }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            if (subpath == "")
            {
                subpath = "/";
            }
            
            return _hybridFileProvider.GetDirectoryContents(subpath);
        }

        public virtual IChangeToken Watch(string filter)
        {
            return _hybridFileProvider.Watch(filter);
        }

        protected virtual IFileProvider CreateHybridProvider(IDynamicFileProvider dynamicFileProvider)
        {
            var fileProviders = new List<IFileProvider>();

            fileProviders.Add(dynamicFileProvider);

            foreach (var fileSet in _options.FileSets.AsEnumerable().Reverse())
            {
                fileProviders.Add(fileSet.FileProvider);
            }

            return new CompositeFileProvider(fileProviders);
        }
    }
}