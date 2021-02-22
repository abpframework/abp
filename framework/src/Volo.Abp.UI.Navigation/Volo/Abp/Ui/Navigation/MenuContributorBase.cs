using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Volo.Abp.UI.Navigation
{
    public abstract class MenuContributorBase : IMenuContributor
    {
        public List<string> PreCheckPermissions { get; }
        public IReadOnlyList<string> GrantedPermissions => _grantedPermissions.ToImmutableList();
        private readonly List<string> _grantedPermissions;

        protected MenuContributorBase()
        {
            PreCheckPermissions = new List<string>();
            _grantedPermissions = new List<string>();
        }

        public void AddGrantedPermission(string permission)
        {
            _grantedPermissions.Add(permission);
        }

        public abstract Task ConfigureMenuAsync(MenuConfigurationContext context);
    }
}
