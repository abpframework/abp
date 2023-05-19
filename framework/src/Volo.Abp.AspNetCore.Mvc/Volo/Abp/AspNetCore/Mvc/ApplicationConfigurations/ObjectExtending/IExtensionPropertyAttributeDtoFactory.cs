using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

public interface IExtensionPropertyAttributeDtoFactory
{
    ExtensionPropertyAttributeDto Create(Attribute attribute);
}
