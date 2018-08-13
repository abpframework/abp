using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure.Internal
{
    public class AzureListFileWrapper : FileInfoBase
    {
        private readonly ICloudBlob _blob;
        private readonly AzureListDirectoryWrapper _parent;

        public AzureListFileWrapper(ICloudBlob blob, AzureListDirectoryWrapper parent)
        {
            _blob = blob;
            var lastSlash = blob.Name.LastIndexOf('/');
            Name = lastSlash >= 0 ? blob.Name.Substring(lastSlash + 1) : blob.Name;

            _parent = parent;
        }

        public override string FullName => _blob.Name;

        public override string Name { get; }

        public override DirectoryInfoBase ParentDirectory => _parent;
    }
}