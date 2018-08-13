using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure.Internal
{
    public class AzureListDirectoryWrapper : DirectoryInfoBase
    {
        private readonly Dictionary<string, AzureBlobReference> _files;

        public AzureListDirectoryWrapper(FileSystemInfoBase childrens)
        {
            FullName = "root";
            ParentDirectory = null;
        }

        public AzureListDirectoryWrapper(string path, Dictionary<string, AzureBlobReference> files)
        {
            var path1 = path ?? "";
            _files = files;
            FullName = path1;
            var lastSlash = path1.LastIndexOf('/');
            Name = lastSlash >= 0 ? path?.Substring(lastSlash + 1) : path;
        }

        public AzureListDirectoryWrapper(CloudBlobDirectory blobDirectory, AzureListDirectoryWrapper parent = null)
        {
            ParentDirectory = parent;
            FullName = blobDirectory.Prefix;
        }

        public override string FullName { get; }

        public override string Name { get; }

        public override DirectoryInfoBase ParentDirectory { get; }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos()
        {
            return _files.Values.Select(file => new AzureListFileWrapper(file.CloudBlob, this));
        }

        public override DirectoryInfoBase GetDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase GetFile(string path)
        {
            return new AzureListFileWrapper(_files[path].CloudBlob, this);
        }
    }
}