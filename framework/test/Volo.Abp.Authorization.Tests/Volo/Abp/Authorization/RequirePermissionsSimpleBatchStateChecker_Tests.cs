using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SimpleStateChecking;
using Xunit;

namespace Volo.Abp.Authorization
{
    public class RequirePermissionsSimpleBatchStateChecker_Tests : AuthorizationTestBase
    {
        private readonly ISimpleStateCheckerManager<MyStateEntity> _simpleStateCheckerManager;

        public RequirePermissionsSimpleBatchStateChecker_Tests()
        {
            _simpleStateCheckerManager = GetRequiredService<ISimpleStateCheckerManager<MyStateEntity>>();
        }

        [Fact]
        public void Switch_Current_Checker_Test()
        {
            var checker = RequirePermissionsSimpleBatchStateChecker<MyStateEntity>.Current;

            using (RequirePermissionsSimpleBatchStateChecker<MyStateEntity>.Use(new RequirePermissionsSimpleBatchStateChecker<MyStateEntity>()))
            {
                RequirePermissionsSimpleBatchStateChecker<MyStateEntity>.Current.ShouldNotBeNull();
                RequirePermissionsSimpleBatchStateChecker<MyStateEntity>.Current.ShouldNotBe(checker);
            }
        }

        [Fact]
        public async Task RequirePermissionsSimpleBatchStateChecker_Test()
        {
            var myStateEntities = new MyStateEntity[]
            {
                new MyStateEntity().RequirePermissions(requiresAll: true, batchCheck:true, permissions: "MyPermission3"),
                new MyStateEntity().RequirePermissions(requiresAll: true, batchCheck:true, permissions: "MyPermission4"),
                new MyStateEntity().RequirePermissions(requiresAll: true, batchCheck:true, permissions: "MyPermission4"),
                new MyStateEntity().RequirePermissions(requiresAll: true, batchCheck:true, permissions: "MyPermission5"),
            };

            var result = await _simpleStateCheckerManager.IsEnabledAsync(myStateEntities);

            result.Count.ShouldBe(myStateEntities.Length);

            result[myStateEntities[0]].ShouldBeTrue();
            result[myStateEntities[1]].ShouldBeFalse();
            result[myStateEntities[2]].ShouldBeFalse();
            result[myStateEntities[3]].ShouldBeTrue();
        }

        class MyStateEntity : IHasSimpleStateCheckers<MyStateEntity>
        {
            public List<ISimpleStateChecker<MyStateEntity>> StateCheckers { get; }

            public MyStateEntity()
            {
                StateCheckers = new List<ISimpleStateChecker<MyStateEntity>>();
            }
        }
    }
}
