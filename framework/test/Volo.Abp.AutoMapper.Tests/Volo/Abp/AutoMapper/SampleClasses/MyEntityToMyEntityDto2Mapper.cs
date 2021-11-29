using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyEntityToMyEntityDto2Mapper : IObjectMapper<MyEntity, MyEntityDto2>, ITransientDependency
    {
        public MyEntityDto2 Map(MyEntity source)
        {
            return new MyEntityDto2
            {
                Id = source.Id,
                Number = source.Number + 1
            };
        }

        public MyEntityDto2 Map(MyEntity source, MyEntityDto2 destination)
        {
            destination.Id = source.Id;
            destination.Number = source.Number + 1;
            return destination;
        }
    }
}