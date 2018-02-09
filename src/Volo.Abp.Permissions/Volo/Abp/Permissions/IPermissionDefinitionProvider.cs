namespace Volo.Abp.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(IPermissionDefinitionContext context);
    }
}