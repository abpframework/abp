using System;
using AutoMapper;
using Shouldly;
using Volo.Abp.Auditing;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapperExpressionExtensions_Tests
    {
        [Fact]
        public void Should_Ignore_Configured_Property()
        {
            var mapper = CreateMapper(
                cfg => cfg.CreateMap<SimpleClass1, SimpleClass2>()
                    .Ignore(x => x.Value2)
                    .Ignore(x => x.Value3)
            );

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

        [Fact]
        public void Should_Ignore_CreationTime()
        {
            var mapper = CreateMapper(
                cfg => cfg.CreateMap<SimpleClassWithCreationTime1, SimpleClassWithCreationTime2>()
                    .IgnoreCreationTime()
            );

            var obj2 = mapper.Map<SimpleClassWithCreationTime2>(
                new SimpleClassWithCreationTime1
                {
                    CreationTime = DateTime.Now
                }
            );
            
            obj2.CreationTime.ShouldBe(default);
        }

        private static IMapper CreateMapper(Action<IMapperConfigurationExpression> configurer)
        {
            var configuration = new MapperConfiguration(configurer);
            configuration.AssertConfigurationIsValid();
            return configuration.CreateMapper();
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

        public class SimpleClassWithCreationTime1 : IHasCreationTime
        {
            public DateTime CreationTime { get; set; }
        }
        
        public class SimpleClassWithCreationTime2 : IHasCreationTime
        {
            public DateTime CreationTime { get; set; }
        }
    }
}