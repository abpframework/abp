using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Permissions;

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

            var identityGroup = context.AddGroup("Identity");

            var userManagement = identityGroup.AddPermission("Identity.UserManagement");
            userManagement.AddChild("Identity.UserManagement.Create");
            userManagement.AddChild("Identity.UserManagement.Update");
            userManagement.AddChild("Identity.UserManagement.Delete");
        }
    }
}
