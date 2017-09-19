using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class UrlActionNameNormalizerContext
    {
        public string RootPath { get; }

        public string ControllerName { get; }

        public ActionModel Action { get; }

        public string HttpMethod { get; }

        public string ActionNameInUrl { get; set; }

        public UrlActionNameNormalizerContext(string rootPath, string controllerName, ActionModel action, string httpMethod, string actionNameInUrl)
        {
            RootPath = rootPath;
            ControllerName = controllerName;
            Action = action;
            HttpMethod = httpMethod;

            ActionNameInUrl = actionNameInUrl;
        }
    }
}