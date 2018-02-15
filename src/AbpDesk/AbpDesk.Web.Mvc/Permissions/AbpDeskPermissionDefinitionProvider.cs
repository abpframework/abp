using Volo.Abp.Permissions;

namespace AbpDesk.Web.Mvc.Permissions
{
    public class AbpDeskPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        public void Define(IPermissionDefinitionContext context)
        {
            var tickets = context.Add("AbpDesk.Tickets");
            tickets.AddChild("AbpDesk.Tickets.Reply");
            tickets.AddChild("AbpDesk.Tickets.Close");

            var customers = context.Add("AbpDesk.Customers");
            customers.AddChild("AbpDesk.Customers.Create");
            customers.AddChild("AbpDesk.Customers.Delete");
        }
    }
}
