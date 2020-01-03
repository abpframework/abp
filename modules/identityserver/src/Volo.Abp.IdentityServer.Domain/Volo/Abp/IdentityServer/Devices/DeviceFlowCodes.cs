using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.Devices
{
    public class DeviceFlowCodes : CreationAuditedAggregateRoot<Guid>
    {
        public virtual string DeviceCode { get; set; }

        public virtual string UserCode { get; set; }

        public virtual string SubjectId { get; set; }

        public virtual string ClientId { get; set; }

        public virtual DateTime? Expiration { get; set; }

        public virtual string Data { get; set; }

        private DeviceFlowCodes()
        {

        }

        public DeviceFlowCodes(Guid id)
        : base(id)
        {

        }
    }
}