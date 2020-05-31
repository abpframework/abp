using AutoMapper;
using Shouldly;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapperExpressionExtensions_Tests
    {
        [Fact]
        public void Should_Ignore_Configured_Property()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.CreateMap<SimpleClass1, SimpleClass2>()
                    .Ignore(x => x.Value2)
                    .Ignore(x => x.Value3)
            );

            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();

            var obj2 = mapper.Map<SimpleClass2>(
                new SimpleClass1
                {
                    Value1 = "v1",
                    Value2 = "v2"
                }
            );
            
            obj2.Value1.ShouldBe("v1");
            obj2.Value2.ShouldBeNull();
            obj2.Value3.ShouldBeNull();
        }

        public class SimpleClass1
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class SimpleClass2
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Value3 { get; set; }
        }
    }
}