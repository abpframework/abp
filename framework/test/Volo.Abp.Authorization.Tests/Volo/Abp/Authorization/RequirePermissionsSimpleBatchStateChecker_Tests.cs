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
            var checker = RequirePermissionsSimpleBatchStateChecker<MyStateEntity2>.Current;
            checker.ShouldNotBeNull();

            RequirePermissionsSimpleBatchStateChecker<MyStateEntity2> checker2 = null;

            using (RequirePermissionsSimpleBatchStateChecker<MyStateEntity2>.Use(new RequirePermissionsSimpleBatchStateChecker<MyStateEntity2>()))
            {
                checker2 = RequirePermissionsSimpleBatchStateChecker<MyStateEntity2>.Current;
                checker2.ShouldNotBeNull();
                checker2.ShouldNotBe(checker);
            }

            checker2.ShouldNotBeNull();
            checker2.ShouldNotBe(checker);
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

        class MyStateEntity2 : IHasSimpleStateCheckers<MyStateEntity2>
        {
            public List<ISimpleStateChecker<MyStateEntity2>> StateCheckers { get; }

            public MyStateEntity2()
            {
                StateCheckers = new List<ISimpleStateChecker<MyStateEntity2>>();
            }
        }
    }
}
