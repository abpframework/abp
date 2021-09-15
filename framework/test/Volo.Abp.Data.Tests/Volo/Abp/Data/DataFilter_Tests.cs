using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Data
{
    public class DataFilter_Tests : AbpIntegratedTest<DataFilter_Tests.TestModule>
    {
        private readonly IDataFilter _dataFilter;
        private readonly AbpDataFilterOptions _dataFilterOptions;

        public DataFilter_Tests()
        {
            _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
            _dataFilterOptions = ServiceProvider
                .GetRequiredService<IOptions<AbpDataFilterOptions>>().Value;
        }

        [Fact]
        public void Should_Allow_Default_Filter_State_To_Be_Updated()
        {
            _dataFilter.ReadOnlyFilters.ContainsKey(typeof(ISoftDelete)).ShouldBe(false);
            _dataFilter.ReadOnlyFilters.ContainsKey(typeof(IGenericDataFilter1)).ShouldBe(false);

            _dataFilterOptions.DefaultFilterState = false;
            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(false);

            _dataFilterOptions.DefaultFilterState = true;
            _dataFilter.IsEnabled<IGenericDataFilter1>().ShouldBe(true);

            _dataFilter.IsEnabled<IDefaultDisabledFilter>().ShouldBe(false);
        }

        [Fact]
        public void Should_Correctly_Dispose_Filter_State()
        {
            void task()
            {
                _dataFilter.IsActive<IGenericDataFilter1>().ShouldBe(false);
                _dataFilter.IsEnabled<IGenericDataFilter1>().ShouldBe(true);

                // filter now cached, is it still marked IsActive == false?
                _dataFilter.IsActive<IGenericDataFilter1>().ShouldBe(false);

                using (_dataFilter.Disable<IGenericDataFilter1>())
                {
                    _dataFilter.IsEnabled<IGenericDataFilter1>().ShouldBe(false);
                    _dataFilter.IsActive<IGenericDataFilter1>().ShouldBe(true);
                }

                _dataFilter.IsActive<IGenericDataFilter1>().ShouldBe(false);
                _dataFilter.IsEnabled<IGenericDataFilter1>().ShouldBe(true);
            }

            // Simulate multiple 'requests' happening at once.
            // The filter state should not be contaminated between requests.
            Parallel.Invoke(task, task, task, task);
        }

        [Fact]
        public void Should_Override_Default_Filter_State_For_Single_Filter()
        {
            _dataFilterOptions.DefaultFilterState.ShouldBe(true);

            _dataFilterOptions.DefaultStates.ContainsKey(typeof(IGenericDataFilter1)).ShouldBe(false);

            _dataFilterOptions.DefaultStates.Add(typeof(IGenericDataFilter1), new DataFilterState(false));

            _dataFilter.IsEnabled<IGenericDataFilter1>().ShouldBe(false);
            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);
        }

        [Fact]
        public void Should_Disable_Filter()
        {
            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);

            using (_dataFilter.Disable<ISoftDelete>())
            {
                _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(false);
            }

            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);
        }

        [Fact]
        public async Task Should_Handle_Nested_Filters()
        {
            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);

            using (_dataFilter.Disable<ISoftDelete>())
            {
                await Task.Run(() =>
                {
                    using (_dataFilter.Enable<ISoftDelete>())
                    {
                        _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);
                    }
                });

                _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(false);
            }

            _dataFilter.IsEnabled<ISoftDelete>().ShouldBe(true);
        }

        [Fact]
        public void Should_Handle_Dynamic_Filters()
        {
            _dataFilter.IsActive(typeof(ICustomFilter<TestSoftDeleteClass>)).ShouldBe(false);
            _dataFilter.IsEnabled(typeof(ICustomFilter<TestSoftDeleteClass>)).ShouldBe(true);

            using (_dataFilter.Disable<ICustomFilter<TestSoftDeleteClass>>())
            {
                _dataFilter.IsActive(typeof(ICustomFilter<TestSoftDeleteClass>)).ShouldBe(true);
                _dataFilter.IsEnabled(typeof(ICustomFilter<TestSoftDeleteClass>)).ShouldBe(false);
            }

            _dataFilter.IsEnabled<ICustomFilter<TestSoftDeleteClass>>().ShouldBe(true);

            // null
            ShouldThrowExtensions.ShouldThrow(
                () => _dataFilter.GetOrAddFilter(null),
                typeof(AbpException)
            );
            // Not an interface
            ShouldThrowExtensions.ShouldThrow(
                () => _dataFilter.GetOrAddFilter(typeof(TestSoftDeleteClass)),
                typeof(AbpException)
            );
            // No type parameter defined
            ShouldThrowExtensions.ShouldThrow(
                () => _dataFilter.GetOrAddFilter(typeof(ICustomFilter<>)),
                typeof(AbpException)
            );
            // Nested generics
            ShouldThrowExtensions.ShouldThrow(
                () => _dataFilter.GetOrAddFilter(typeof(ICustomFilter<ICustomFilter<TestSoftDeleteClass>>)),
                typeof(AbpException)
            );
            // Multiple type parameters
            ShouldThrowExtensions.ShouldThrow(
                () => _dataFilter.GetOrAddFilter(typeof(ICustomFilter<SimpleClass, SimpleClass>)),
                typeof(AbpException)
            );
        }

        [Fact]
        public void Should_Work_Without_DataFilter_Container()
        {
            var genericDataFilter2 = ServiceProvider.GetRequiredService<IDataFilter<IGenericDataFilter2>>();

            genericDataFilter2.IsActive.ShouldBe(false);
            genericDataFilter2.IsEnabled.ShouldBe(true);

            using (genericDataFilter2.Disable())
            {
                genericDataFilter2.IsActive.ShouldBe(true);
                genericDataFilter2.IsEnabled.ShouldBe(false);

                using (genericDataFilter2.Enable())
                {
                    genericDataFilter2.IsActive.ShouldBe(true);
                    genericDataFilter2.IsEnabled.ShouldBe(true);
                }

                genericDataFilter2.IsActive.ShouldBe(true);
            }

            genericDataFilter2.IsActive.ShouldBe(false);
            genericDataFilter2.IsEnabled.ShouldBe(true);
        }

        class TestSoftDeleteClass : ISoftDelete, ITestMarkerInterface
        {
            public bool IsDeleted { get; set; }
        }
        interface ITestMarkerInterface { }

        interface ICustomFilter<T> { }
        interface ICustomFilter<T,T2> { }
        class SimpleClass { }

        interface IDefaultDisabledFilter { }
        interface IGenericDataFilter1 { }
        interface IGenericDataFilter2 { }


        [DependsOn(typeof(AbpDataModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                base.ConfigureServices(context);

                Configure<AbpDataFilterOptions>(options =>
                {
                    options.DefaultStates.Add(typeof(IDefaultDisabledFilter), new DataFilterState(false));
                });
            }

        }
    }
}
