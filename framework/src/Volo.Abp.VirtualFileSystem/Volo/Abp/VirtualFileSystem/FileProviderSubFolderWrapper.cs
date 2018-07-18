using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Volo.Abp.VirtualFileSystem
{
    public class FileProviderSubFolderWrapper : IFileProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly string _contentPath;

        public FileProviderSubFolderWrapper(IFileProvider fileProvider, string contentPath = null)
        {
            _contentPath = contentPath;
            _fileProvider = fileProvider;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(NormalizePath(subpath));
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents(NormalizePath(subpath));
        }

        private string NormalizePath(string subpath)
        {
            return _contentPath == null
                ? subpath
                : _contentPath + subpath;
        }

        public IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch(filter); //TODO: Why this does not use NormalizePath?
        }
    }
}