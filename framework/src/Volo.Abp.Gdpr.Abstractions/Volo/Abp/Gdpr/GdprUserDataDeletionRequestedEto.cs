using System;

namespace Volo.Abp.Gdpr;

[Serializable]
public class GdprUserDataDeletionRequestedEto
{
    public Guid UserId { get; set; }
}