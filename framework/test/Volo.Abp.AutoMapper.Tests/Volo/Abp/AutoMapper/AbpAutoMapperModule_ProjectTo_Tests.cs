using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperModule_ProjectTo_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AbpAutoMapperModule_ProjectTo_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void ProjectTo_Test()
        {
            var myEntities = new List<MyEntity>
            {
                new MyEntity
                {
                    Number = 42
                }
            };

            var myEntityDtos = _objectMapper.ProjectTo<MyEntityDto>(myEntities.AsQueryable()).ToList();
            myEntityDtos.ShouldNotBeNull();
            myEntityDtos.Count.ShouldBe(1);
            myEntityDtos.ShouldContain(x => x.Number == 42);
        }
    }
}
