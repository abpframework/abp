namespace Volo.Abp.Permissions
{
    public interface IPermissionDefinitionContext
    {
        PermissionDefinition GetOrNull(string name);

        void Add(params PermissionDefinition[] definitions);
    }
}