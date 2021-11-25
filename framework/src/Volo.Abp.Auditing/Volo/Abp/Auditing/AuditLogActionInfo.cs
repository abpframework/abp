using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.Auditing;

[Serializable]
public class AuditLogActionInfo : IHasExtraProperties
{
    public string ServiceName { get; set; }

    public string MethodName { get; set; }

    public string Parameters { get; set; }

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    public AuditLogActionInfo()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }
}
