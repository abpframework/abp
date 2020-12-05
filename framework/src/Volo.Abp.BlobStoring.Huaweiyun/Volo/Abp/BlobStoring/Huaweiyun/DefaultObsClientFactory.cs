using System;
using System.Net.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class DefaultObsClientFactory : IObsClientFactory, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultObsClientFactory(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual IObsClient Create(HuaweiyunBlobProviderConfiguration args)
        {
            return new ObsClient(_serviceProvider, args.Endpoint, args.AccessKeyId, args.SecretAccessKey);
        }
    }
}
