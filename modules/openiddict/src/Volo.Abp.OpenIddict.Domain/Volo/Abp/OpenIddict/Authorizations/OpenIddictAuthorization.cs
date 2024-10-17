using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Timing;

namespace Volo.Abp.OpenIddict.Authorizations;

public class OpenIddictAuthorization : AggregateRoot<Guid>
{
    public OpenIddictAuthorization()
    {
    }

    public OpenIddictAuthorization(Guid id)
        : base(id)
    {
    }

    /// <summary>
    /// Gets or sets the application associated with the current authorization.
    /// </summary>
    public virtual Guid? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the UTC creation date of the current authorization.
    /// </summary>
    [DisableDateTimeNormalization]
    public virtual DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <c>null</c> if no bag was associated with the current authorization.
    /// </summary>
    public virtual string Properties { get; set; }

    /// <summary>
    /// Gets or sets the scopes associated with the current
    /// authorization, serialized as a JSON array.
    /// </summary>
    public virtual string Scopes { get; set; }

    /// <summary>
    /// Gets or sets the status of the current authorization.
    /// </summary>
    public virtual string Status { get; set; }

    /// <summary>
    /// Gets or sets the subject associated with the current authorization.
    /// </summary>
    public virtual string Subject { get; set; }

    /// <summary>
    /// Gets or sets the type of the current authorization.
    /// </summary>
    public virtual string Type { get; set; }
}
