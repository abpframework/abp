using System.Collections.Generic;

namespace Volo.Abp.DynamicProxy;

public class ScopeEventLog
{
    public List<string> Events { get; } = new();

    public int TargetInvocations;
}
