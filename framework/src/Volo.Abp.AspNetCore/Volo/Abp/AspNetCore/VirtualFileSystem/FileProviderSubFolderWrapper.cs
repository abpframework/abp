//using System;
//using System.Linq;
//using JetBrains.Annotations;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Primitives;

//namespace Volo.Abp.VirtualFileSystem
//{
//    //TODO: Ensure that merging such subfolders is secure! (anyone can add /.. to the beginning!)..?

//    public class FileProviderSubFolderWrapper : IFileProvider
//    {
//        private readonly IFileProvider _fileProvider;
//        private readonly string _rootPath;
//        private readonly string[] _allowedExtraFolders;
//        private readonly string[] _allowedExtraFileExtensions;

//        public FileProviderSubFolderWrapper(
//            IFileProvider fileProvider,
//            string rootPath,
//            string[] allowedExtraFolders = null,
//            string[] allowedExtraFileExtensions = null)
//        {
//            _fileProvider = fileProvider;
//            _rootPath = rootPath;
//            _allowedExtraFileExtensions = allowedExtraFileExtensions ?? Array.Empty<string>();
//            _allowedExtraFolders = allowedExtraFolders ?? Array.Empty<string>();
//        }

//        public IFileInfo GetFileInfo(string subpath)
//        {
//            Check.NotNullOrEmpty(subpath, nameof(subpath));

//            if (ExtraAllowedPath(subpath))
//            {
//                var fileInfo = _fileProvider.GetFileInfo(subpath);
//                if (fileInfo.Exists)
//                {
//                    return fileInfo;
//                }
//            }

//            return _fileProvider.GetFileInfo(_rootPath + subpath);
//        }

//        public IDirectoryContents GetDirectoryContents([NotNull] string subpath)
//        {
//            Check.NotNullOrEmpty(subpath, nameof(subpath));

//            if (ExtraAllowedPath(subpath))
//            {
//                var directory = _fileProvider.GetDirectoryContents(subpath);
//                if (directory.Exists)
//                {
//                    return directory;
//                }
//            }

//            return _fileProvider.GetDirectoryContents(_rootPath + subpath);
//        }

//        private bool ExtraAllowedPath(string path)
//        {
//            return ExtraAllowedFolder(path) && ExtraAllowedExtension(path);
//        }

//        private bool ExtraAllowedFolder(string path)
//        {
//            return _allowedExtraFolders.Any(s => path.StartsWith(s, StringComparison.OrdinalIgnoreCase));
//        }

//        private bool ExtraAllowedExtension(string path)
//        {
//            return _allowedExtraFileExtensions.Any(e => path.EndsWith(e, StringComparison.OrdinalIgnoreCase));
//        }

//        public IChangeToken Watch(string filter)
//        {
//            return _fileProvider.Watch(filter); //TODO: Why this does not use NormalizePath?
//        }
//    }
//}