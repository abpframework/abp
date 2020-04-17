namespace Volo.Abp.UI.Navigation.Urls
{
    public class AppUrlOptions
    {
        public ApplicationUrlDictionary Applications { get; }

        public AppUrlOptions()
        {
            Applications = new ApplicationUrlDictionary();
        }
    }
}
