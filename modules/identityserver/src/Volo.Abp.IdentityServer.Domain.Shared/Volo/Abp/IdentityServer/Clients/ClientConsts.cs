namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientConsts
    {
        public static int ClientIdMaxLength { get; set; } =  200;

        public static int ProtocolTypeMaxLength { get; set; } =  200;

        public static int ClientNameMaxLength { get; set; } =  200;

        public static int ClientUriMaxLength { get; set; } =  2000;

        public static int LogoUriMaxLength { get; set; } =  2000;

        public static int DescriptionMaxLength { get; set; } =  1000;

        public static int FrontChannelLogoutUriMaxLength { get; set; } =  2000;

        public static int BackChannelLogoutUriMaxLength { get; set; } =  2000;

        public static int ClientClaimsPrefixMaxLength { get; set; } =  200;

        public static int PairWiseSubjectSaltMaxLength { get; set; } =  200;

        public static int UserCodeTypeMaxLength { get; set; } =  100;

        public static int AllowedIdentityTokenSigningAlgorithms { get; set; } =  100;
    }
}
