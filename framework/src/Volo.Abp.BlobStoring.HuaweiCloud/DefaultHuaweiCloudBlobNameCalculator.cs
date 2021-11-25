using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public class DefaultHuaweiCloudBlobNameCalculator : ITransientDependency, IHuaweiCloudBlobNameCalculator
    {
        protected ICurrentTenant CurrentTenant { get; }

        public DefaultHuaweiCloudBlobNameCalculator(ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public virtual string Calculate(BlobProviderArgs args)
        {
            return CurrentTenant.Id == null
                ? $"host/{args.BlobName}"
                : $"tenants/{CurrentTenant.Id.Value.ToString("D")}/{args.BlobName}";
        }
    }
}
