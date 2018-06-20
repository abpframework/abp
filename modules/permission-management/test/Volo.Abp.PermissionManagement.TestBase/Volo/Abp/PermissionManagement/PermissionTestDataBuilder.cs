using System;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionTestDataBuilder : ITransientDependency
    {
        public static Guid User1Id { get; } = Guid.NewGuid();
        public static Guid User2Id { get; } = Guid.NewGuid();

        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PermissionTestDataBuilder(IGuidGenerator guidGenerator, IPermissionGrantRepository permissionGrantRepository)
        {
            _guidGenerator = guidGenerator;
            _permissionGrantRepository = permissionGrantRepository;
        }

        public void Build()
        {
            _permissionGrantRepository.Insert(
                new PermissionGrant(
                    _guidGenerator.Create(),
                    "MyPermission1",
                    UserPermissionValueProvider.ProviderName,
                    User1Id.ToString()
                )
            );
        }
    }
}