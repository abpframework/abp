namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(IPermissionDefinitionContext context);
    }
}