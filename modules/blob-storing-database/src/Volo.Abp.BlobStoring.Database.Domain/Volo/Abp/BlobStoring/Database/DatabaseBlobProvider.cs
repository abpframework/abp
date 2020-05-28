using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Database
{
    public class DatabaseBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IBlobRepository BlobRepository { get; }
        protected IContainerRepository ContainerRepository { get; }

        public DatabaseBlobProvider(IBlobRepository blobRepository, IContainerRepository containerRepository)
        {
            BlobRepository = blobRepository;
            ContainerRepository = containerRepository;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var container = await ContainerRepository.CreateIfNotExistAsync(args.ContainerName, args.TenantId, args.CancellationToken);
            
            var blob = await BlobRepository.FindAsync(container.Id, args.BlobName, args.TenantId, args.CancellationToken);

            var content = await args.BlobStream.GetAllBytesAsync(args.CancellationToken);

            if (blob != null)
            {
                if (!args.OverrideExisting)
                {
                    throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
                }

                blob.Content = content;
                await BlobRepository.UpdateAsync(blob);
            }
            else
            {
                blob = new Blob(Guid.NewGuid(), container.Id, args.BlobName, content, args.TenantId);
                await BlobRepository.InsertAsync(blob);
            }
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var container = await ContainerRepository.FindAsync(args.ContainerName, args.TenantId, args.CancellationToken);

            if (container == null)
            {
                return false;
            }

            return await BlobRepository.DeleteAsync(container.Id, args.BlobName, args.TenantId, args.CancellationToken);
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var container = await ContainerRepository.FindAsync(args.ContainerName, args.TenantId, args.CancellationToken);

            if (container == null)
            {
                return false;
            }

            return await BlobRepository.ExistsAsync(container.Id, args.BlobName, args.TenantId, args.CancellationToken);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var container = await ContainerRepository.FindAsync(args.ContainerName, args.TenantId, args.CancellationToken);

            if (container == null)
            {
                return null;
            }
            
            var blob = await BlobRepository.FindAsync(container.Id, args.BlobName, args.TenantId, args.CancellationToken);

            if (blob == null)
            {
                return null;
            }

            return new MemoryStream(blob.Content);
        }
    }
}