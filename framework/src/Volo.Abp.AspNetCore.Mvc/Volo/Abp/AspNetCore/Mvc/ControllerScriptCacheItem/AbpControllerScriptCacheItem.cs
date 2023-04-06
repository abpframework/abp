namespace Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;

public class AbpControllerScriptCacheItem
{
    public string Script { get; set; }

    public AbpControllerScriptCacheItem(string script)
    {
        Script = script;
    }
}
