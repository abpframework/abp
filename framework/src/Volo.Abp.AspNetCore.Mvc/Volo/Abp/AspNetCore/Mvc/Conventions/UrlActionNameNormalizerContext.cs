using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class UrlActionNameNormalizerContext
{
    public string RootPath { get; }

    public string ControllerName { get; }

    public ActionModel Action { get; }

    public string ActionNameInUrl { get; }

    public string HttpMethod { get; }

    public UrlActionNameNormalizerContext(string rootPath, string controllerName, ActionModel action, string actionNameInUrl, string httpMethod)
    {
        RootPath = rootPath;
        ControllerName = controllerName;
        Action = action;
        ActionNameInUrl = actionNameInUrl;
        HttpMethod = httpMethod;
    }
}
