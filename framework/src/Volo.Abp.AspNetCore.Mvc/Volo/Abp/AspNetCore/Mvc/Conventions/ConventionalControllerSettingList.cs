using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class ConventionalControllerSettingList : List<ConventionalControllerSetting>
{
    [CanBeNull]
    public ConventionalControllerSetting GetSettingOrNull(Type controllerType)
    {
        return this.FirstOrDefault(controllerSetting => controllerSetting.ControllerTypes.Contains(controllerType));
    }
}
