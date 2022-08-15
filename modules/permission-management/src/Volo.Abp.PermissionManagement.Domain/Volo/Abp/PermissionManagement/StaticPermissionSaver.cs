using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.PermissionManagement;

public class StaticPermissionSaver : IStaticPermissionSaver, ITransientDependency
{
    protected IStaticPermissionDefinitionStore StaticStore { get; }
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; }
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; }
    protected IPermissionDefinitionSerializer PermissionSerializer { get; }

    public StaticPermissionSaver(
        IStaticPermissionDefinitionStore staticStore,
        IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
        IPermissionDefinitionRecordRepository permissionRepository,
        IPermissionDefinitionSerializer permissionSerializer)
    {
        StaticStore = staticStore;
        PermissionGroupRepository = permissionGroupRepository;
        PermissionRepository = permissionRepository;
        PermissionSerializer = permissionSerializer;
    }
    
    [UnitOfWork]
    public virtual async Task SaveAsync()
    {
        // TODO: Save only changed permissions & groups
        /* + get all groups & perms
         * - compare and update or insert groups
         * - get all permissions
         * - compare and update or insert permissions
         * - set in-memory cache using the latest group and permission data
         */

        var (permissionGroupRecords, permissionRecords) = await PermissionSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );
        
        
    }
}