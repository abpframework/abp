using System.Security.Claims;

namespace Volo.Abp.Security.Claims;

//TODO: Instead of directly using static properties, can we just create an AbpClaimOptions and pass these values as defaults?
/// <summary>
/// Used to get ABP-specific claim type names.
/// </summary>
public static class AbpClaimTypes
{
    /// <summary>
    /// Default: <see cref="ClaimTypes.Name"/>
    /// </summary>
    public static string UserName { get; set; } = ClaimTypes.Name;

    /// <summary>
    /// Default: <see cref="ClaimTypes.GivenName"/>
    /// </summary>
    public static string Name { get; set; } = ClaimTypes.GivenName;

    /// <summary>
    /// Default: <see cref="ClaimTypes.Surname"/>
    /// </summary>
    public static string SurName { get; set; } = ClaimTypes.Surname;

    /// <summary>
    /// Default: <see cref="ClaimTypes.NameIdentifier"/>
    /// </summary>
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

    /// <summary>
    /// Default: <see cref="ClaimTypes.Role"/>
    /// </summary>
    public static string Role { get; set; } = ClaimTypes.Role;

    /// <summary>
    /// Default: <see cref="ClaimTypes.Email"/>
    /// </summary>
    public static string Email { get; set; } = ClaimTypes.Email;

    /// <summary>
    /// Default: "email_verified".
    /// </summary>
    public static string EmailVerified { get; set; } = "email_verified";

    /// <summary>
    /// Default: "phone_number".
    /// </summary>
    public static string PhoneNumber { get; set; } = "phone_number";

    /// <summary>
    /// Default: "phone_number_verified".
    /// </summary>
    public static string PhoneNumberVerified { get; set; } = "phone_number_verified";

    /// <summary>
    /// Default: "tenantid".
    /// </summary>
    public static string TenantId { get; set; } = "tenantid";

    /// <summary>
    /// Default: "editionid".
    /// </summary>
    public static string EditionId { get; set; } = "editionid";

    /// <summary>
    /// Default: "client_id".
    /// </summary>
    public static string ClientId { get; set; } = "client_id";

    /// <summary>
    /// Default: "impersonator_tenantid".
    /// </summary>
    public static string ImpersonatorTenantId { get; set; } = "impersonator_tenantid";

    /// <summary>
    /// Default: "impersonator_userid".
    /// </summary>
    public static string ImpersonatorUserId { get; set; } = "impersonator_userid";
}
