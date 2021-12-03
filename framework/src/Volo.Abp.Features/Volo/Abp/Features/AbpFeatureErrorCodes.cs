namespace Volo.Abp.Features;

public static class AbpFeatureErrorCodes
{
    public const string FeatureIsNotEnabled = "Volo.Feature:010001";

    public const string AllOfTheseFeaturesMustBeEnabled = "Volo.Feature:010002";

    public const string AtLeastOneOfTheseFeaturesMustBeEnabled = "Volo.Feature:010003";
}
