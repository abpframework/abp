using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    //TODO: Re-consider properties of this class.
    public class UrlControllerNameNormalizerContext
    {
        public string RootPath { get; }

        public string ControllerName { get; }

        public ActionModel Action { get; }

        public string HttpMethod { get; }

        public UrlControllerNameNormalizerContext(string rootPath, string controllerName, ActionModel action, string httpMethod)
        {
            RootPath = rootPath;
            ControllerName = controllerName;
            Action = action;
            HttpMethod = httpMethod;
        }
    }
}