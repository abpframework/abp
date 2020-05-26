using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.BlobStoring.FileSystem
{
    //TODO: What if the file is being used on create, delete or read?

    public class FileSystemBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IBlogFilePathCalculator FilePathCalculator { get; }
        
        public FileSystemBlobProvider(IBlogFilePathCalculator filePathCalculator)
        {
            FilePathCalculator = filePathCalculator;
        }
        
        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args);
            
            DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(filePath));

            var fileMode = args.OverrideExisting
                ? FileMode.Create
                : FileMode.CreateNew;
            
            using (var fileStream = File.Open(filePath, fileMode, FileAccess.Write))
            {
                //TODO: Truely implement this (like this? http://writeasync.net/?p=2621 or https://www.infoworld.com/article/2995387/how-to-perform-asynchronous-file-operations-in-c.html)
                
                await args.BlobStream.CopyToAsync(
                    fileStream,
                    81920, //this is already the default value, but needed to set to be able to pass the cancellationToken
                    args.CancellationToken
                );

                await fileStream.FlushAsync();
            }
        }

        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args);
            
            return Task.FromResult(FileHelper.DeleteIfExists(filePath));
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args);

            return Task.FromResult(File.Exists(filePath));
        }

        public override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args);

            if (!File.Exists(filePath))
            {
                return Task.FromResult<Stream>(null);
            }

            return Task.FromResult<Stream>(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
    }
}