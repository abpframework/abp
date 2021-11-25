using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Volo.Abp.AspNetCore.Mvc;

public class AspNetCoreApiDescriptionModelProviderOptions
{
    public Func<Type, ConventionalControllerSetting, string> ControllerNameGenerator { get; set; }

    public Func<MethodInfo, string> ActionNameGenerator { get; set; }

    public Func<ApiParameterDescription, string> ApiParameterNameGenerator { get; set; }

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

        ApiParameterNameGenerator = (apiParameterDescription) =>
        {
            if (apiParameterDescription.ModelMetadata is DefaultModelMetadata defaultModelMetadata)
            {
                var jsonPropertyNameAttribute = (System.Text.Json.Serialization.JsonPropertyNameAttribute)
                    defaultModelMetadata?.Attributes?.PropertyAttributes?.FirstOrDefault(x => x is System.Text.Json.Serialization.JsonPropertyNameAttribute);
                if (jsonPropertyNameAttribute != null)
                {
                    return jsonPropertyNameAttribute.Name;
                }

                var jsonPropertyAttribute = (Newtonsoft.Json.JsonPropertyAttribute)
                    defaultModelMetadata?.Attributes?.PropertyAttributes?.FirstOrDefault(x => x is Newtonsoft.Json.JsonPropertyAttribute);
                if (jsonPropertyAttribute != null)
                {
                    return jsonPropertyAttribute.PropertyName;
                }
            }

            return null;
        };
    }
}
