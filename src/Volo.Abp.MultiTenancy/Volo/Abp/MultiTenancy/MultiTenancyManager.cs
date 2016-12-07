using System;
using System.Collections.Generic;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantInfo
    {
        Guid Id { get; }

        string Name { get; }
    }

    public interface IMultiTenancyManager
    {
        ITenantInfo CurrentTenant { get; }
    }

    public class MultiTenancyManager : IMultiTenancyManager
    {
        public ITenantInfo CurrentTenant => GetCurrentTenant();

        private readonly IEnumerable<ICurrentTenantResolver> _currentTenantResolvers;

        public MultiTenancyManager(IEnumerable<ICurrentTenantResolver> currentTenantResolvers)
        {
            _currentTenantResolvers = currentTenantResolvers;
        }

        public virtual ITenantInfo GetCurrentTenant()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICurrentTenantResolver
    {
        void Resolve(ICurrentTenantResolveContext context);
    }

    public interface ICurrentTenantResolveContext
    {

    }
}
