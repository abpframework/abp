using Volo.Abp.Localization;

namespace Volo.Abp.ExceptionHandling
{
    public interface ILocalizeErrorMessage
    {
        string LocalizeMessage(LocalizationContext context);
    }
}