using Volo.Abp.Collections;

namespace Volo.Abp.Authorization.Permissions
{
    public class AbpPermissionOptions
    {
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IPermissionValueProvider> ValueProviders { get; }

        public ITypeList<IPermissionStateProvider> GlobalStateProviders { get; }

        public AbpPermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
            ValueProviders = new TypeList<IPermissionValueProvider>();
            GlobalStateProviders = new TypeList<IPermissionStateProvider>();
        }
    }
}
