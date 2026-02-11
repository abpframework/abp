using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class AbpConventionalControllerOptions
{
    public ConventionalControllerSettingList ConventionalControllerSettings { get; }

    public List<Type> FormBodyBindingIgnoredTypes { get; }

    /// <summary>
    /// Set true to use the old style URL path style.
    /// Default: false.
    /// </summary>
    public bool UseV3UrlStyle { get; set; }

    public string[] IgnoredUrlSuffixesInControllerNames { get; set; } = new[] { "Integration" };

    public AbpConventionalControllerOptions()
    {
        ConventionalControllerSettings = new ConventionalControllerSettingList();

        FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile),
                typeof(IRemoteStreamContent)
            };
    }

    public AbpConventionalControllerOptions Create(
        Assembly assembly,
        Action<ConventionalControllerSetting>? optionsAction = null)
    {
        var setting = new ConventionalControllerSetting(
            assembly,
            ModuleApiDescriptionModel.DefaultRootPath,
            ModuleApiDescriptionModel.DefaultRemoteServiceName
        );

        optionsAction?.Invoke(setting);
        setting.Initialize();
        ConventionalControllerSettings.Add(setting);
        return this;
    }
}
