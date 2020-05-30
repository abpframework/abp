using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.BlobStoring.Database
{
    public class DatabaseBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IDatabaseBlobRepository DatabaseBlobRepository { get; }
        protected IDatabaseBlobContainerRepository DatabaseBlobContainerRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public DatabaseBlobProvider(
            IDatabaseBlobRepository databaseBlobRepository,
            IDatabaseBlobContainerRepository databaseBlobContainerRepository,
            IGuidGenerator guidGenerator)
        {
            DatabaseBlobRepository = databaseBlobRepository;
            DatabaseBlobContainerRepository = databaseBlobContainerRepository;
            GuidGenerator = guidGenerator;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var container = await GetOrCreateContainerAsync(args.ContainerName, args.TenantId, args.CancellationToken);

            var blob = await DatabaseBlobRepository.FindAsync(container.Id, args.BlobName, args.TenantId,
                args.CancellationToken);

            var content = await args.BlobStream.GetAllBytesAsync(args.CancellationToken);

            if (blob != null)
            {
                if (!args.OverrideExisting)
                {
                    throw new BlobAlreadyExistsException(
                        $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
                }

                blob.SetContent(content);
                await DatabaseBlobRepository.UpdateAsync(blob);
            }
            else
            {
                blob = new DatabaseBlob(GuidGenerator.Create(), container.Id, args.BlobName, content, args.TenantId);
                await DatabaseBlobRepository.InsertAsync(blob);
            }
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var container =
                await DatabaseBlobContainerRepository.FindAsync(args.ContainerName, args.TenantId,
                    args.CancellationToken);

            if (container == null)
            {
                return false;
            }

            return await DatabaseBlobRepository.DeleteAsync(container.Id, args.BlobName, args.TenantId,
                args.CancellationToken);
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var container =
                await DatabaseBlobContainerRepository.FindAsync(args.ContainerName, args.TenantId,
                    args.CancellationToken);

            if (container == null)
            {
                return false;
            }

            return await DatabaseBlobRepository.ExistsAsync(container.Id, args.BlobName, args.TenantId,
                args.CancellationToken);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var container =
                await DatabaseBlobContainerRepository.FindAsync(args.ContainerName, args.TenantId,
                    args.CancellationToken);

            if (container == null)
            {
                return null;
            }

            var blob = await DatabaseBlobRepository.FindAsync(container.Id, args.BlobName, args.TenantId,
                args.CancellationToken);

            if (blob == null)
            {
                return null;
            }

            return new MemoryStream(blob.Content);
        }

        protected virtual async Task<DatabaseBlobContainer> GetOrCreateContainerAsync(
            string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var container = await DatabaseBlobContainerRepository.FindAsync(name, tenantId, cancellationToken);
            if (container != null)
            {
                return container;
            }

            container = new DatabaseBlobContainer(GuidGenerator.Create(), name, tenantId);
            await DatabaseBlobContainerRepository.InsertAsync(container, cancellationToken: cancellationToken);

            return container;
        }
    }
}