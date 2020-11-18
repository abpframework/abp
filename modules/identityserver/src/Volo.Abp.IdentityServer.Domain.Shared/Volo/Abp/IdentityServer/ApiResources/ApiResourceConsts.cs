namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceConsts
    {
        public static int NameMaxLength { get; set; } = 200;

        public static int DisplayNameMaxLength { get; set; } = 200;

        public static int DescriptionMaxLength { get; set; } = 1000;

        public static int AllowedAccessTokenSigningAlgorithmsMaxLength { get; set; } = 100;
    }
}
