using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Permissions
{
    public class AbpPermissionTestDataBuilder : ITransientDependency
    {
        public static Guid User1Id = Guid.NewGuid();
        public static Guid User2Id = Guid.NewGuid();

        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AbpPermissionTestDataBuilder(IGuidGenerator guidGenerator, IPermissionGrantRepository permissionGrantRepository)
        {
            _guidGenerator = guidGenerator;
            _permissionGrantRepository = permissionGrantRepository;
        }

        public void Build()
        {

        }
    }
}