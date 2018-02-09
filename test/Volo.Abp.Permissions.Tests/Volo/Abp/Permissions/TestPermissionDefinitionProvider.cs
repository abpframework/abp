namespace Volo.Abp.Permissions
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myPermission1 = new PermissionDefinition("MyPermission1");
            var myPermission2 = new PermissionDefinition("MyPermission2");

            context.Add(myPermission1, myPermission2);
        }
    }
}