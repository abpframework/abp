namespace Volo.Abp.Authorization
{
    public static class AbpAuthorizationErrorCodes
    {
        public const string GivenPolicyHasNotGranted = "Volo.Authorization:010001";

        public const string GivenPolicyHasNotGrantedWithPolicyName = "Volo.Authorization:010002";

        public const string GivenPolicyHasNotGrantedForGivenResource = "Volo.Authorization:010003";

        public const string GivenRequirementHasNotGrantedForGivenResource = "Volo.Authorization:010004";

        public const string GivenRequirementsHasNotGrantedForGivenResource = "Volo.Authorization:010005";
    }
}
