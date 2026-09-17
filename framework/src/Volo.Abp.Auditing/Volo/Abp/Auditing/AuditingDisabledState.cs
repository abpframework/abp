namespace Volo.Abp.Auditing;

public class AuditingDisabledState
{
    public bool IsDisabled { get; private set; }

    public AuditingDisabledState(bool isDisabled)
    {
        IsDisabled = isDisabled;
    }
}
