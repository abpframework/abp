using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Uow
{
    public class UnitOfWork_Nested_Tests : AbpIntegratedTest<IndependentEmptyModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWork_Nested_Tests()
        {
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Create_Nested_UnitOfWorks()
        {
            _unitOfWorkManager.Current.ShouldBeNull();

            using (var uow1 = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.ShouldBe(uow1);

                using (var uow2 = _unitOfWorkManager.BeginNew())
                {
                    _unitOfWorkManager.Current.ShouldNotBeNull();
                    _unitOfWorkManager.Current.Id.ShouldNotBe(uow1.Id);

                    await uow2.CompleteAsync();
                }

                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.ShouldBe(uow1);

                await uow1.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldBeNull();
        }
    }
}
