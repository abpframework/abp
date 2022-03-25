using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.Authorizations;

public class OpenIddictAuthorization : FullAuditedEntity<Guid>
{
    /// <summary>
    /// Gets or sets the application associated with the current authorization.
    /// </summary>
    public virtual Guid? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the UTC creation date of the current authorization.
    /// </summary>
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
    /// Gets the list of tokens associated with the current authorization.
    /// </summary>
    public virtual ICollection<OpenIddictToken> Tokens { get; } = new HashSet<OpenIddictToken>();

    /// <summary>
    /// Gets or sets the type of the current authorization.
    /// </summary>
    public virtual string Type { get; set; }
}
