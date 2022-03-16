using System;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging;

public class TestData : ISingletonDependency
{
    public int HandledAuditLogEventCount { get; set; } = 0;
    public Guid UserId { get; set; } = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
    public AuditLogInfo UserLogInfo { get; set; }
    public AuditLogInfoEto UserLogInfoEto { get; set; }
}