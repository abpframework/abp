using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiLingualObject.TestObjects;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.MultiLingualObject
{
    public class MultiLingualObject_Tests : AbpIntegratedTest<AbpMultiLingualObjectTestModule>
    {
        private readonly IObjectMapper<MultiLingualBook, MultiLingualBookDto> _objectMapper;
        private readonly MultiLingualBook _book;

        public MultiLingualObject_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper<MultiLingualBook, MultiLingualBookDto>>();

            var id = Guid.NewGuid();
            _book = new MultiLingualBook(id, 100)
            {
                Translations = new List<MultiLingualBookTranslation>
                {
                    new MultiLingualBookTranslation
                    {
                        CoreId = id,
                        Language = "en",
                        Name = "C# in Depth"
                    },
                    new MultiLingualBookTranslation
                    {
                        CoreId = id,
                        Language = "zh-Hans",
                        Name = "深入理解C#"
                    }
                }
            };
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        [Fact]
        public void Should_Map_Current_UI_Culture()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var bookDto = _objectMapper.Map(_book);

                bookDto.Name.ShouldBe("深入理解C#");
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }

        [Fact]
        public void Should_Map_Fallback_UI_Culture()
        {
            using (CultureHelper.Use("en-us"))
            {
                var bookDto = _objectMapper.Map(_book);

                bookDto.Name.ShouldBe("C# in Depth");
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }

        [Fact]
        public void Should_Map_Default_Language()
        {
            using (CultureHelper.Use("tr"))
            {
                var bookDto = _objectMapper.Map(_book);

                bookDto.Name.ShouldBe("C# in Depth");
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }
    }

    public class MultiLingualBookObjectMapper : IObjectMapper<MultiLingualBook, MultiLingualBookDto>,
        ITransientDependency
    {
        private readonly ISettingProvider _settingProvider;

        public MultiLingualBookObjectMapper(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public MultiLingualBookDto Map(MultiLingualBook source)
        {
            var translation = source.GetMultiLingualTranslation(_settingProvider, true);

            return new MultiLingualBookDto
            {
                Id = source.Id,
                Name = translation.Name,
                Price = source.Price
            };
        }

        public MultiLingualBookDto Map(MultiLingualBook source, MultiLingualBookDto destination)
        {
            return default;
        }
    }
}
