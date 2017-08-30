using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public interface IAbpControllerAssemblySettingBuilder
    {
        AbpControllerAssemblySettingBuilder Where(Func<Type, bool> predicate);

        AbpControllerAssemblySettingBuilder ConfigureControllerModel(Action<ControllerModel> configurer);
    }
}