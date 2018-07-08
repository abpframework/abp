using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.Auditing
{
    public class AuditInfo : IHasExtraProperties
    {
        public Guid? TenantId { get; set; }
        
        public Guid? UserId { get; set; }

        public Guid? ImpersonatorUserId { get; set; }

        public Guid? ImpersonatorTenantId { get; set; }

        public string ServiceName { get; set; }
        
        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public string ClientIpAddress { get; set; }
        
        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public Exception Exception { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public AuditInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            var loggedUserId = UserId.HasValue
                                   ? "user " + UserId.Value
                                   : "an anonymous user";

            var exceptionOrSuccessMessage = Exception != null
                ? "exception: " + Exception.Message
                : "succeed";

            return $"AUDIT LOG: {ServiceName}.{MethodName} is executed by {loggedUserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address with {exceptionOrSuccessMessage}.";
        }
    }
}