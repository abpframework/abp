using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class DefaultHuaweiyunBlobNameCalculator : IHuaweiyunBlobNameCalculator, ITransientDependency
    {

        protected ICurrentTenant CurrentTenant { get; }

        public DefaultHuaweiyunBlobNameCalculator(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            return CurrentTenant.Id == null
                ? $"host/{args.BlobName}"
                : $"tenants/{CurrentTenant.Id.Value:D}/{args.BlobName}";
        }
    }
}
