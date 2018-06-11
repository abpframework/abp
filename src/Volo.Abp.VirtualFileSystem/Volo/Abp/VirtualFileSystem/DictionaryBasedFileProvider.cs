using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Volo.Abp.VirtualFileSystem
{
    public abstract class DictionaryBasedFileProvider : IFileProvider
    {
        protected abstract IDictionary<string, IFileInfo> Files { get; }

        public virtual IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var file = Files.GetOrDefault(NormalizePath(subpath));

            if (file == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            return file;
        }

        public virtual IDirectoryContents GetDirectoryContents(string subpath)
        {
            var directory = GetFileInfo(subpath);
            if (!directory.IsDirectory)
            {
                return new NotFoundDirectoryContents();
            }

            var fileList = new List<IFileInfo>();

            var directoryPath = subpath.EnsureEndsWith('/');
            foreach (var fileInfo in Files.Values)
            {
                if (!fileInfo.PhysicalPath.StartsWith(directoryPath))
                {
                    continue;
                }

                var relativePath = fileInfo.PhysicalPath.Substring(directoryPath.Length);
                if (relativePath.Contains("/"))
                {
                    continue;
                }

                fileList.Add(fileInfo);
            }

            return new EnumerableDirectoryContents(fileList);
        }

        public virtual IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        protected virtual string NormalizePath(string subpath)
        {
            return subpath;
        }
    }
}