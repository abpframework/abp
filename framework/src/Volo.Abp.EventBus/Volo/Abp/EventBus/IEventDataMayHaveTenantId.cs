using System;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// An event data object (or event transfer object) can implement this interface
    /// to indicate that this event may be related to a tenant.
    ///
    /// If an event data class is always related to a tenant, then directly implement the
    /// <see cref="IsMultiTenant"/> interface instead of this one.
    ///
    /// This interface is typically implemented by generic event handlers where the generic
    /// parameter may implement <see cref="IsMultiTenant"/> or not.
    /// </summary>
    public interface IEventDataMayHaveTenantId
    {
        /// <summary>
        /// Returns true if this event data has a Tenant Id information.
        /// If so, it should set the <paramref name="tenantId"/> our parameter.
        /// Otherwise, the <paramref name="tenantId"/> our parameter value should not be informative
        /// (it will be null as expected, but doesn't indicate a tenant with null tenant id).
        /// </summary>
        /// <param name="tenantId">
        /// The tenant id that is set if this method returns true.
        /// </param>
        bool IsMultiTenant(out Guid? tenantId);
    }
}
