using JetBrains.Annotations;

namespace Volo.Abp.Ui.Navigation.Urls
{
    public interface IAppUrlProvider
    {
        string GetUrl([NotNull] string appName, [CanBeNull] string urlName = null);
    }
}
