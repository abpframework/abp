﻿using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IPermissionDefinitionContext context);
    }
}