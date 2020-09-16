namespace Volo.Abp.Ui.Branding
{
    public interface IBrandingProvider
    {
        string AppName { get; }

        /// <summary>
        /// Logo on white background
        /// </summary>
        string LogoUrl { get; }

        /// <summary>
        /// Logo on dark background
        /// </summary>
        string LogoReverseUrl { get; }
    }
}
