using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    public interface IConventionalRouteBuilder
    {
        string Build(
            string rootPath,
            string controllerName,
            ActionModel action,
            string httpMethod,
            [CanBeNull] ConventionalControllerSetting configuration
        );
    }
}
