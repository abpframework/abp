using System;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;

namespace Volo.Abp.OpenIddict.Tokens;

/* TODO: Reconsider the entity designs
 *
 * Entity structure seems incorrectly designed:
 *   `OpenIddictToken` is an entity (not aggregate root) but have its own repository.
 *   `OpenIddictAuthorization` is also same.
 *     If they have repositories, they should be aggregate roots and should not be used as navigation properties in other entities (e.g. `OpenIddictApplication`).
 *     I think this will be the correct way since they are independently queried from database.
 */

public class OpenIddictToken : FullAuditedEntity<Guid>, IHasConcurrencyStamp
{
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }
    
    /// <summary>
    /// Gets or sets the application associated with the current token.
    /// </summary>
    public virtual Guid? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the authorization associated with the current token.
    /// </summary>
    public virtual Guid? AuthorizationId { get; set; }

    /// <summary>
    /// Gets or sets the UTC creation date of the current token.
    /// </summary>
    public virtual DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the UTC expiration date of the current token.
    /// </summary>
    public virtual DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the payload of the current token, if applicable.
    /// Note: this property is only used for reference tokens
    /// and may be encrypted for security reasons.
    /// </summary>
    public virtual string Payload { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <c>null</c> if no bag was associated with the current token.
    /// </summary>
    public virtual string Properties { get; set; }

    /// <summary>
    /// Gets or sets the UTC redemption date of the current token.
    /// </summary>
    public virtual DateTime? RedemptionDate { get; set; }

    /// <summary>
    /// Gets or sets the reference identifier associated
    /// with the current token, if applicable.
    /// Note: this property is only used for reference tokens
    /// and may be hashed or encrypted for security reasons.
    /// </summary>
    public virtual string ReferenceId { get; set; }

    /// <summary>
    /// Gets or sets the status of the current token.
    /// </summary>
    public virtual string Status { get; set; }

    /// <summary>
    /// Gets or sets the subject associated with the current token.
    /// </summary>
    public virtual string Subject { get; set; }

    /// <summary>
    /// Gets or sets the type of the current token.
    /// </summary>
    public virtual string Type { get; set; }
}
