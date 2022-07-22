namespace Volo.Abp.AspNetCore.Mvc.NewtonsoftJson;

public class AbpAspNetCoreMvcNewtonsoftOptions
{
    public bool UseHybridSerializer  { get; set; }

    public AbpAspNetCoreMvcNewtonsoftOptions()
    {
        UseHybridSerializer = true;
    }
}
