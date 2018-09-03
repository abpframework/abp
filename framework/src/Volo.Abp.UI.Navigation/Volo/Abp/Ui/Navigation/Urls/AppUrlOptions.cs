using Volo.Abp.Ui.Navigation.Urls;

namespace Volo.Abp.Ui.Navigation.Urls
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
