namespace Volo.Abp.AspNetCore.Builder
{
    public interface IConfigureAspNet
    {
        void Configure(AspNetConfigurationContext context);
    }
}
