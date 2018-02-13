namespace Volo.Abp.Permissions
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            context.Add("MyPermission1");

            var myPermission2 = context.Add("MyPermission2");
            myPermission2.AddChild("MyPermission2.ChildPermission1");
        }
    }
}