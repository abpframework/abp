namespace Volo.Abp.AspNetCore.Mvc
{
    public class UrlControllerNameNormalizerContext
    {
        public string RootPath { get; }

        public string ControllerName { get; }

        public UrlControllerNameNormalizerContext(string rootPath, string controllerName)
        {
            RootPath = rootPath;
            ControllerName = controllerName;
        }
    }
}