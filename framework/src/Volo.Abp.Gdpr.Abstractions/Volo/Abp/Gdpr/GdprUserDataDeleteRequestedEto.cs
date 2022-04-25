using System;

namespace Volo.Abp.Gdpr;

[Serializable]
public class GdprUserDataDeleteRequestedEto
{
    public Guid UserId { get; set; }
}