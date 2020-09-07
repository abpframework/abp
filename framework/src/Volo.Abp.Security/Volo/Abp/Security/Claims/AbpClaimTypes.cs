using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    //TODO: Instead of directly using static properties, can we just create an AbpClaimOptions and pass these values as defaults?
    /// <summary>
    /// Used to get ABP-specific claim type names.
    /// </summary>
    public static class AbpClaimTypes
    {
        public const string ClaimTypeNamespace = "abp_";

        /// <summary>
        /// Default: abp_username
        /// </summary>
        public static string UserName { get; set; } = ClaimTypeNamespace + "username";

        /// <summary>
        /// Default: abp_name
        /// </summary>
        public static string Name { get; set; } = ClaimTypeNamespace + "name";

        /// <summary>
        /// Default: abp_surname
        /// </summary>
        public static string SurName { get; set; } = ClaimTypeNamespace + "surname";

        /// <summary>
        /// Default: abp_user_id
        /// </summary>
        public static string UserId { get; set; } = ClaimTypeNamespace + "user_id";

        /// <summary>
        /// Default: abp_role
        /// </summary>
        public static string Role { get; set; } = ClaimTypeNamespace + "role";

        /// <summary>
        /// Default: abp_security_stamp
        /// </summary>
        public static string SecurityStamp { get; set; } =  ClaimTypeNamespace + "security_stamp";

        /// <summary>
        /// Default: abp_email
        /// </summary>
        public static string Email { get; set; } = ClaimTypeNamespace + "email";

        /// <summary>
        /// Default: abp_email_verified
        /// </summary>
        public static string EmailVerified { get; set; } = ClaimTypeNamespace + "email_verified";

        /// <summary>
        /// Default: abp_phone_number
        /// </summary>
        public static string PhoneNumber { get; set; } = ClaimTypeNamespace + "phone_number";

        /// <summary>
        /// Default: abp_phone_number_verified
        /// </summary>
        public static string PhoneNumberVerified { get; set; } = ClaimTypeNamespace + "phone_number_verified";

        /// <summary>
        /// Default: abp_tenant_id
        /// </summary>
        public static string TenantId { get; set; } = ClaimTypeNamespace + "tenant_id";

        /// <summary>
        /// Default: abp_edition_id
        /// </summary>
        public static string EditionId { get; set; } = ClaimTypeNamespace + "edition_id";

        /// <summary>
        /// Default: abp_client_id
        /// </summary>
        public static string ClientId { get; set; } = ClaimTypeNamespace + "client_id";
    }
}
