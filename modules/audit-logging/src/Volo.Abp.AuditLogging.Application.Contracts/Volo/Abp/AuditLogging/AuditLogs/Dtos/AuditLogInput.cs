/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos/AuditLogInput
* 创建者：天上有木月
* 创建时间：2019/4/2 2:33:06
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System.Net;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos
{
    public class AuditLogInput : PagedAndSortedResultRequestDto
    {
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string ApplicationName { get; set; }
        public string CorrelationId { get; set; }
        public int? MaxExecutionDuration { get; set; }
        public int? MinExecutionDuration { get; set; }
        public bool? HasException { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }
        public bool IncludeDetails { get; set; }

    }
}
