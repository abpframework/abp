using System;

namespace Volo.Abp.Auditing;

[Serializable]
public class EntityPropertyChangeInfoEto
{
    public string NewValue { get; set; }

    public string OriginalValue { get; set; }

    public string PropertyName { get; set; }

    public string PropertyTypeFullName { get; set; }
}