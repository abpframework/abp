namespace Volo.Abp.Account.Web
{
    public class ConsentOptions
    {
        public static bool EnableOfflineAccess = true;

        public static string OfflineAccessDisplayName = "Offline Access";

        public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

        //TODO: How to handle this
        public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";

        //TODO: How to handle this
        public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
    }
}
