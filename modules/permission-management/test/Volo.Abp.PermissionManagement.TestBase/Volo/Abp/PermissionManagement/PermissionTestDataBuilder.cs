using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.PermissionManagement;

public class PermissionTestDataBuilder : ITransientDependency
{
    public static Guid User1Id { get; } = Guid.NewGuid();
    public static Guid User2Id { get; } = Guid.NewGuid();

    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly IResourcePermissionGrantRepository _resourcePermissionGrantRepository;
    private readonly IGuidGenerator _guidGenerator;

    public PermissionTestDataBuilder(
        IGuidGenerator guidGenerator,
        IPermissionGrantRepository permissionGrantRepository,
        IResourcePermissionGrantRepository resourcePermissionGrantRepository)
    {
        _guidGenerator = guidGenerator;
        _permissionGrantRepository = permissionGrantRepository;
        _resourcePermissionGrantRepository = resourcePermissionGrantRepository;
    }

    public async Task BuildAsync()
    {
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                "MyPermission1",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                "TestEntityManagementPermission",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                "MyDisabledPermission1",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                "MyPermission3",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                "MyPermission5",
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyResourcePermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyDisabledResourcePermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyResourcePermission3",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey3,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyResourcePermission5",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey3,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyResourcePermission6",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey3,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );

        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                _guidGenerator.Create(),
                "MyResourcePermission5",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey5,
                UserPermissionValueProvider.ProviderName,
                User1Id.ToString()
            )
        );
    }
}
