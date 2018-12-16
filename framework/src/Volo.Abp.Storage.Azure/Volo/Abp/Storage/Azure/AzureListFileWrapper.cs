using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Volo.Abp.Storage.Azure
{
    public class AzureListFileWrapper : FileInfoBase
    {
        private readonly ICloudBlob _blob;
        private readonly AzureListDirectoryWrapper _parent;
        private readonly string _name;

        public AzureListFileWrapper(ICloudBlob blob, AzureListDirectoryWrapper parent)
        {
            _blob = blob;

            var lastSlash = blob.Name.LastIndexOf('/');
            _name = lastSlash >= 0 ? blob.Name.Substring(lastSlash + 1) : blob.Name;

            _parent = parent;
        }

        public override string FullName => _blob.Name;

        public override string Name => _name;

        public override DirectoryInfoBase ParentDirectory => _parent;
    }
}