using System;

namespace Volo.Abp.Auditing;

[Serializable]
public class EntityPropertyChangeInfo
{
    /// <summary>
    /// Maximum length of <see cref="PropertyName"/> property.
    /// Value: 96.
    /// </summary>
    public const int MaxPropertyNameLength = 96;

    /// <summary>
    /// Maximum length of <see cref="NewValue"/> and <see cref="OriginalValue"/> properties.
    /// Value: 512.
    /// </summary>
    public const int MaxValueLength = 512;

    /// <summary>
    /// Maximum length of <see cref="PropertyTypeFullName"/> property.
    /// Value: 512.
    /// </summary>
    public const int MaxPropertyTypeFullNameLength = 192;

    public virtual string NewValue { get; set; }

    public virtual string OriginalValue { get; set; }

    public virtual string PropertyName { get; set; }

    public virtual string PropertyTypeFullName { get; set; }
}
