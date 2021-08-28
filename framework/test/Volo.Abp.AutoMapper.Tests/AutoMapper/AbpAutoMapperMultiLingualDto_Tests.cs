using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.MultiLingualObjects.TestObjects;
using Volo.Abp.Testing;
using Xunit;

namespace AutoMapper
{
    public class AbpAutoMapperMultiLingualDto_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly Volo.Abp.ObjectMapping.IObjectMapper _objectMapper;
        private readonly MultiLingualBook _book;

        public AbpAutoMapperMultiLingualDto_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<Volo.Abp.ObjectMapping.IObjectMapper>();

            var id = Guid.NewGuid();
            _book = new MultiLingualBook(id, "C# in Depth",100)
            {
                DefaultCulture = "en",
                Translations = new TranslationDictionary()
            };
            
            var tr = new MultiLingualBookTranslation
            {
                Culture = "tr",
                Name = "C# Derinlemesine",
            };

            var zh = new MultiLingualBookTranslation
            {
                Culture = "zh-Hans",
                Name = "深入理解C#",
            };

            _book.Translations.Add(zh.Culture, zh);
            _book.Translations.Add(tr.Culture, tr);
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
            using (CultureHelper.Use("en-GB"))
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
            using (CultureHelper.Use("ar"))
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

                bookDto.Name.ShouldBe("C# in Depth");
                bookDto.Price.ShouldBe(_book.Price);
                bookDto.Id.ShouldBe(_book.Id);
            }
        }
    }

    public class BookProfile : Profile
    {
        public BookProfile()
        {
            var mapResult = this.CreateMultiLingualMap<MultiLingualBook, MultiLingualBookTranslation, MultiLingualBookDto>();

            mapResult.EntityMap.ValidateMemberList(MemberList.None);
            mapResult.TranslateMap.ValidateMemberList(MemberList.None);
        }
    }
}
