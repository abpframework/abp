using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Permissions
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public PermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator)
        {
            GuidGenerator = guidGenerator;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
        }

        public async Task<bool> IsGrantedAsync(string providerName, string providerKey, string name)
        {
            Check.NotNull(providerName, nameof(providerName));
            Check.NotNull(providerKey, nameof(providerKey));
            Check.NotNull(name, nameof(name));

            return await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null;
        }

        public async Task<List<string>> GetAllGrantedAsync(string providerName, string providerKey)
        {
            Check.NotNull(providerName, nameof(providerName));
            Check.NotNull(providerKey, nameof(providerKey));

            return (await PermissionGrantRepository.GetListAsync(providerName, providerKey))
                .Select(p => p.Name)
                .ToList();
        }

        public async Task GrantAsync(string name, string providerName, string providerKey)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));
            Check.NotNull(providerKey, nameof(providerKey));

            if (await IsGrantedAsync(providerName, providerKey, name))
            {
                return;
            }

            await PermissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    GuidGenerator.Create(),
                    name,
                    providerName,
                    providerKey
                )
            );
        }

        public async Task RevokeAsync(string providerName, string providerKey, string name)
        {
            Check.NotNull(providerName, nameof(providerName));
            Check.NotNull(providerKey, nameof(providerKey));
            Check.NotNull(name, nameof(name));

            if (await IsGrantedAsync(providerName, providerKey, name))
            {
                return;
            }

            var grant = await PermissionGrantRepository.FindAsync(name, providerName, providerKey);
            if (grant == null)
            {
                return;
            }

            await PermissionGrantRepository.DeleteAsync(grant);
        }
    }
}