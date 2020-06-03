using System.IO;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.FileSystem
{
    public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultBlobFilePathCalculator(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }
        
        public virtual string Calculate(BlobProviderArgs args)
        {
            var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
            var blobPath = fileSystemConfiguration.BasePath;

            if (CurrentTenant.Id == null)
            {
                blobPath = Path.Combine(blobPath, "host");
            }
            else
            {
                blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
            }

            if (fileSystemConfiguration.AppendContainerNameToBasePath)
            {
                blobPath = Path.Combine(blobPath, args.ContainerName);
            }
            
            blobPath = Path.Combine(blobPath, args.BlobName);

            return blobPath;
        }
    }
}