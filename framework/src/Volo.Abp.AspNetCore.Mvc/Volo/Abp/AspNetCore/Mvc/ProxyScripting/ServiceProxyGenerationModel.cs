using System;
using System.Linq;
using Volo.Abp.Http.ProxyScripting;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;

namespace Volo.Abp.AspNetCore.Mvc.ProxyScripting;

public class ServiceProxyGenerationModel //: TODO: IShouldNormalize
{
    public string Type { get; set; }

    public bool UseCache { get; set; }

    public string Modules { get; set; }

    public string Controllers { get; set; }

    public string Actions { get; set; }

    public ServiceProxyGenerationModel()
    {
        UseCache = true;
    }

    public void Normalize()
    {
        if (Type.IsNullOrEmpty())
        {
            Type = JQueryProxyScriptGenerator.Name;
        }
    }

    public ProxyScriptingModel CreateOptions()
    {
        var options = new ProxyScriptingModel(Type, UseCache);

        if (!Modules.IsNullOrEmpty())
        {
            options.Modules = Modules.Split('|').Select(m => m.Trim()).ToArray();
        }

        if (!Controllers.IsNullOrEmpty())
        {
            options.Controllers = Controllers.Split('|').Select(m => m.Trim()).ToArray();
        }

        if (!Actions.IsNullOrEmpty())
        {
            options.Actions = Actions.Split('|').Select(m => m.Trim()).ToArray();
        }

        return options;
    }
}
