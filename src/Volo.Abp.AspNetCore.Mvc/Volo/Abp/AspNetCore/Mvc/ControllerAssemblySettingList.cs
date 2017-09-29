using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class ControllerAssemblySettingList : List<AbpControllerAssemblySetting>
    {
        [CanBeNull]
        public AbpControllerAssemblySetting GetSettingOrNull(Type controllerType)
        {
            return this.FirstOrDefault(controllerSetting => controllerSetting.ControllerTypes.Contains(controllerType));
        }
    }
}