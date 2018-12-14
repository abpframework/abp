using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure
{
    public class AzureListDirectoryWrapper : DirectoryInfoBase
    {
        private readonly string _name;
        private readonly string _fullName;
        private readonly string _path;
        private readonly Dictionary<string, AzureFileReference> _files;

        public AzureListDirectoryWrapper(FileSystemInfoBase childrens)
        {
            _fullName = "root";
            ParentDirectory = null;
        }

        public AzureListDirectoryWrapper(string path, Dictionary<string, AzureFileReference> files)
        {
            _path = path ?? "";
            _files = files;
            _fullName = _path;
            var lastSlash = _path.LastIndexOf('/');
            _name = lastSlash >= 0 ? path.Substring(lastSlash + 1) : path;
        }

        public AzureListDirectoryWrapper(CloudBlobDirectory blobDirectory, AzureListDirectoryWrapper parent = null)
        {
            ParentDirectory = parent;
            _fullName = blobDirectory.Prefix;
        }

        public override string FullName => _fullName;

        public override string Name => _name;

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
