using System;
using System.Collections.Generic;

namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenantAccessor
    {
        Guid Id { get; }

        string Name { get; }
    }

    public class CurrentTenantAccessor : ICurrentTenantAccessor
    {
        public Guid Id => GetTenantId();

        public string Name { get; }

        private readonly IEnumerable<ICurrentTenantResolver> _currentTenantResolvers;

        public CurrentTenantAccessor(IEnumerable<ICurrentTenantResolver> currentTenantResolvers)
        {
            _currentTenantResolvers = currentTenantResolvers;
        }

        public virtual Guid GetTenantId()
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
