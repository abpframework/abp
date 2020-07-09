using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.SecurityLog
{
    [Serializable]
    public class SecurityLogInfo
    {
        /// <summary>
        /// The name of the application or service writing user security logs.
        /// Default: null.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Web, JWT, Identity, Identity_Server
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// login_successful, login_failed, logout, change_pwd, refresh_token...
        /// </summary>
        public string Action { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public Guid? TenantId { get; set; }

        public string TenantName { get; set; }

        public string ClientId { get; set; }

        public string CorrelationId { get; set; }

        public string ClientIpAddress { get; set; }

        public string BrowserInfo { get; set; }

        public DateTime CreationTime { get; set; }

        public SecurityLogInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return $"SECURITY LOG: [{ApplicationName} - {Identity} - {Action}]";
        }
    }
}
