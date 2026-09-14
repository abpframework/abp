using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ui.Branding;

public class DefaultBrandingProvider : IBrandingProvider, IBrandingLogoProvider, ITransientDependency
{
    public virtual string AppName => "MyApplication";

    public virtual string? LogoUrl => null;

    public virtual string? LogoReverseUrl => null;

    public virtual string? LogoIconUrl => null;

    public virtual string? LogoIconReverseUrl => null;
}
