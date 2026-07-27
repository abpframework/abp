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

        // A failure is only retried while it is replayable: before OpenFileStream returns
        // (the source is untouched), or for a seekable overwrite (the source can seek back
        // and FileMode.Create truncates the partial content). Otherwise a retry would
        // replay a half-consumed source or hit the file a failed CreateNew attempt left behind.
        long sourcePosition;
        try
        {
            sourcePosition = args.BlobStream.CanSeek && fileMode == FileMode.Create ? args.BlobStream.Position : -1;
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is IOException)
        {
            // A failing position probe degrades to a single, non-replayable attempt
            sourcePosition = -1;
        }

        var targetOpened = false;

        await Policy.Handle<IOException>(_ => sourcePosition >= 0 || !targetOpened)
            .WaitAndRetryAsync(2, retryCount => TimeSpan.FromSeconds(retryCount))
            .ExecuteAsync(async () =>
            {
                if (sourcePosition >= 0)
                {
                    args.BlobStream.Seek(sourcePosition, SeekOrigin.Begin);
                }

                using (var fileStream = OpenFileStream(filePath, fileMode))
                {
                    targetOpened = true;

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

    protected virtual Stream OpenFileStream(string filePath, FileMode fileMode)
    {
        return File.Open(filePath, fileMode, FileAccess.Write);
    }

    protected virtual Task<bool> ExistsAsync(string filePath)
    {
        return Task.FromResult(File.Exists(filePath));
    }
}
