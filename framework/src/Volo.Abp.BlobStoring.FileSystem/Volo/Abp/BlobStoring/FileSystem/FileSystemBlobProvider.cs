using System;
using System.IO;
using System.Threading.Tasks;
using Polly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.BlobStoring.FileSystem;

public class FileSystemBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IBlobFilePathCalculator FilePathCalculator { get; }

    public FileSystemBlobProvider(IBlobFilePathCalculator filePathCalculator)
    {
        FilePathCalculator = filePathCalculator;
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var filePath = FilePathCalculator.Calculate(args);

        if (!args.OverrideExisting && await ExistsAsync(filePath))
        {
            throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
        }

        DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(filePath)!);

        var fileMode = args.OverrideExisting
            ? FileMode.Create
            : FileMode.CreateNew;

        // Retry only replayable overwrites: a non-seekable source would continue from the
        // middle (corrupted file), and a failed CreateNew leaves the file behind anyway
        var sourcePosition = args.BlobStream.CanSeek && fileMode == FileMode.Create ? args.BlobStream.Position : -1;

        await Policy.Handle<IOException>(_ => sourcePosition >= 0)
            .WaitAndRetryAsync(2, retryCount => TimeSpan.FromSeconds(retryCount))
            .ExecuteAsync(async () =>
            {
                if (sourcePosition >= 0)
                {
                    args.BlobStream.Seek(sourcePosition, SeekOrigin.Begin);
                }

                using (var fileStream = File.Open(filePath, fileMode, FileAccess.Write))
                {
                    await args.BlobStream.CopyToAsync(
                        fileStream,
                        args.CancellationToken
                    );

                    await fileStream.FlushAsync();
                }
            });
    }

    public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var filePath = FilePathCalculator.Calculate(args);
        return Task.FromResult(FileHelper.DeleteIfExists(filePath));
    }

    public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var filePath = FilePathCalculator.Calculate(args);
        return ExistsAsync(filePath);
    }

    public override async Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var filePath = FilePathCalculator.Calculate(args);

        if (!File.Exists(filePath))
        {
            return null;
        }

        return await Policy.Handle<IOException>()
            .WaitAndRetryAsync(2, retryCount => TimeSpan.FromSeconds(retryCount))
            .ExecuteAsync(() => Task.FromResult(File.OpenRead(filePath)));
    }

    protected virtual Task<bool> ExistsAsync(string filePath)
    {
        return Task.FromResult(File.Exists(filePath));
    }
}
