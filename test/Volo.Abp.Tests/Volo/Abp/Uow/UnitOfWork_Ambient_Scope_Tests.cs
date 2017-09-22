using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Uow
{
    public class UnitOfWork_Ambient_Scope_Tests : AbpIntegratedTest<IndependentEmptyModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWork_Ambient_Scope_Tests()
        {
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task UnitOfWorkManager_Current_Should_Set_Correctly()
        {
            _unitOfWorkManager.Current.ShouldBeNull();

            using (var uow1 = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.ShouldBe(uow1);

                using (var uow2 = _unitOfWorkManager.Begin())
                {
                    _unitOfWorkManager.Current.ShouldNotBeNull();
                    _unitOfWorkManager.Current.Id.ShouldBe(uow1.Id);

                    await uow2.CompleteAsync();
                }

                await uow1.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldBeNull();
        }
    }
}
