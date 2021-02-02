using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AspNetCoreApiDescriptionModelProviderOptions
    {
        public Func<Type, ConventionalControllerSetting, string> ControllerNameGenerator { get; set; }

        public Func<MethodInfo, string> ActionNameGenerator { get; set; }

        public AspNetCoreApiDescriptionModelProviderOptions()
        {
            ControllerNameGenerator = (controllerType, setting) =>
            {
                var controllerName = controllerType.Name.RemovePostFix("Controller")
                    .RemovePostFix(ApplicationService.CommonPostfixes);

                if (setting?.UrlControllerNameNormalizer != null)
                {
                    controllerName =
                        setting.UrlControllerNameNormalizer(
                            new UrlControllerNameNormalizerContext(setting.RootPath, controllerName));
                }

                return controllerName;
            };

            ActionNameGenerator = (method) =>
            {
                var methodNameBuilder = new StringBuilder(method.Name);

                var parameters = method.GetParameters();
                if (parameters.Any())
                {
                    methodNameBuilder.Append("By");

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (i > 0)
                        {
                            methodNameBuilder.Append("And");
                        }

                        methodNameBuilder.Append(parameters[i].Name.ToPascalCase());
                    }
                }

                return methodNameBuilder.ToString();
            };
        }
    }
}
