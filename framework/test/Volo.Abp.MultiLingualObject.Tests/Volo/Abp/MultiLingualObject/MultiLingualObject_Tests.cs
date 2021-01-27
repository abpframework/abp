using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiLingualObject.TestObjects;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.MultiLingualObject
{
    public class MultiLingualObject_Tests : AbpIntegratedTest<AbpMultiLingualObjectTestModule>
    {
        private readonly IObjectMapper _objectMapper;
        private readonly MultiLingualBook _book;

        public MultiLingualObject_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();

            var id = Guid.NewGuid();
            _book = new MultiLingualBook(id, 100)
            {
                Translations = new List<MultiLingualBookTranslation>()
            };

            var en = new MultiLingualBookTranslation
            {
                Language = "en",
                Name = "C# in Depth",
            };
            var zh = new MultiLingualBookTranslation
            {
                Language = "zh-Hans",
                Name = "深入理解C#",
            };

            _book.Translations.Add(en);
            _book.Translations.Add(zh);
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
                var bookDto = _objectMapper.Map<MultiLingualBook, MultiLingualBookDto>(_book);

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
                var bookDto = _objectMapper.Map<MultiLingualBook, MultiLingualBookDto>(_book);

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
                var bookDto = _objectMapper.Map<MultiLingualBook, MultiLingualBookDto>(_book);

                bookDto.Name.ShouldBe("C# in Depth");
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }

        [Fact]
        public void NoTranslations_ShouldStillMapObject()
        {
            _book.Translations.Clear();

            using (CultureHelper.Use("tr"))
            {
                var bookDto = _objectMapper.Map<MultiLingualBook, MultiLingualBookDto>(_book);

                bookDto.Name.ShouldBeNull();
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }
    }

    public class MultiLingualBookObjectMapper : IObjectMapper<MultiLingualBook, MultiLingualBookDto>,
        ITransientDependency
    {
        private readonly IMultiLingualObjectManager _multiLingualObjectManager;

        public MultiLingualBookObjectMapper(IMultiLingualObjectManager multiLingualObjectManager)
        {
            _multiLingualObjectManager = multiLingualObjectManager;
        }

        public MultiLingualBookDto Map(MultiLingualBook source)
        {
            var translation =
                _multiLingualObjectManager.GetTranslation<MultiLingualBook, MultiLingualBookTranslation>(source);

            return new MultiLingualBookDto
            {
                Price = source.Price,
                Id = source.Id,
                Name = translation?.Name
            };
        }

        public MultiLingualBookDto Map(MultiLingualBook source, MultiLingualBookDto destination)
        {
            return default;
        }
    }
}
