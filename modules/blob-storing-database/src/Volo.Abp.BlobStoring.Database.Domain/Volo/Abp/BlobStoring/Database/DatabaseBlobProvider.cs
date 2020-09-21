using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Database
{
    public class DatabaseBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IDatabaseBlobRepository DatabaseBlobRepository { get; }
        protected IDatabaseBlobContainerRepository DatabaseBlobContainerRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IDataFilter<IMultiTenant> MultiTenantFilter { get; }
        protected IDatabaseBlobNameCalculator DatabaseBlobNameCalculator { get; }

        public DatabaseBlobProvider(
            IDatabaseBlobRepository databaseBlobRepository,
            IDatabaseBlobContainerRepository databaseBlobContainerRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IDataFilter<IMultiTenant> multiTenantFilter,
            IDatabaseBlobNameCalculator databaseBlobNameCalculator)
        {
            DatabaseBlobRepository = databaseBlobRepository;
            DatabaseBlobContainerRepository = databaseBlobContainerRepository;
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
            MultiTenantFilter = multiTenantFilter;
            DatabaseBlobNameCalculator = databaseBlobNameCalculator;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull(args)))
            {
                var container = await GetOrCreateContainerAsync(args);

                var blobName = DatabaseBlobNameCalculator.Calculate(args);

                var blob = await DatabaseBlobRepository.FindAsync(
                    container.Id,
                    blobName,
                    args.CancellationToken
                );

                var content = await args.BlobStream.GetAllBytesAsync(args.CancellationToken);

                if (blob != null)
                {
                    if (!args.OverrideExisting)
                    {
                        throw new BlobAlreadyExistsException(
                            $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
                    }

                    blob.SetContent(content);

                    await DatabaseBlobRepository.UpdateAsync(blob, autoSave: true);
                }
                else
                {
                    blob = new DatabaseBlob(GuidGenerator.Create(), container.Id, blobName, content, CurrentTenant.Id);
                    await DatabaseBlobRepository.InsertAsync(blob, autoSave: true);
                }
            }
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull(args)))
            {
                var container = await DatabaseBlobContainerRepository.FindAsync(
                    args.ContainerName,
                    args.CancellationToken
                );

                if (container == null)
                {
                    return false;
                }

                return await DatabaseBlobRepository.DeleteAsync(
                    container.Id,
                    DatabaseBlobNameCalculator.Calculate(args),
                    autoSave: true,
                    cancellationToken: args.CancellationToken
                );
            }
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull(args)))
            {
                var container = await DatabaseBlobContainerRepository.FindAsync(
                    args.ContainerName,
                    args.CancellationToken
                );

                if (container == null)
                {
                    return false;
                }

                return await DatabaseBlobRepository.ExistsAsync(
                    container.Id,
                    DatabaseBlobNameCalculator.Calculate(args),
                    args.CancellationToken
                );
            }
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull(args)))
            {
                var container = await DatabaseBlobContainerRepository.FindAsync(
                    args.ContainerName,
                    args.CancellationToken
                );

                if (container == null)
                {
                    return null;
                }

                var blob = await DatabaseBlobRepository.FindAsync(
                    container.Id,
                    DatabaseBlobNameCalculator.Calculate(args),
                    args.CancellationToken
                );

                if (blob == null)
                {
                    return null;
                }

                return new MemoryStream(blob.Content);
            }
        }

        protected virtual async Task<DatabaseBlobContainer> GetOrCreateContainerAsync(BlobProviderSaveArgs args)
        {
            var container = await DatabaseBlobContainerRepository.FindAsync(args.ContainerName, args.CancellationToken);
            if (container != null)
            {
                return container;
            }

            container = new DatabaseBlobContainer(GuidGenerator.Create(), args.ContainerName, GetTenantIdOrNull(args));
            await DatabaseBlobContainerRepository.InsertAsync(container, cancellationToken: args.CancellationToken);

            return container;
        }

        protected virtual Guid? GetTenantIdOrNull(BlobProviderArgs args)
        {
            return args.Configuration.IsMultiTenant ? CurrentTenant.Id : null;
        }
    }
}
