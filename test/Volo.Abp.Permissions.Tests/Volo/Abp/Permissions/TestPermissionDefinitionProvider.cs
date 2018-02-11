namespace Volo.Abp.Permissions
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myPermission1 = new PermissionDefinition("MyPermission1");

            context.Add(myPermission1);
        }
    }
}