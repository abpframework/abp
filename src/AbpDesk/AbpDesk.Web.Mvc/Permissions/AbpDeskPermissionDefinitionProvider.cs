using Volo.Abp.Authorization.Permissions;

namespace AbpDesk.Web.Mvc.Permissions
{
    public class AbpDeskPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var abpDeskGroup = context.AddGroup("AbpDesk");

            var tickets = abpDeskGroup.AddPermission("AbpDesk.Tickets");
            tickets.AddChild("AbpDesk.Tickets.Reply");
            tickets.AddChild("AbpDesk.Tickets.Close");

            var customers = abpDeskGroup.AddPermission("AbpDesk.Customers");
            customers.AddChild("AbpDesk.Customers.Create");
            customers.AddChild("AbpDesk.Customers.Delete");
        }
    }
}
