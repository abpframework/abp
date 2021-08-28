using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.MultiLingualObjects.TestObjects;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.MultiLingualObjects
{
    public class MultiLingualObjectManager_Tests : AbpIntegratedTest<AbpMultiLingualObjectsTestModule>
    {
        private readonly IMultiLingualObjectManager _multiLingualObjectManager;
        private readonly MultiLingualBook _book;

        public MultiLingualObjectManager_Tests()
        {
            _multiLingualObjectManager = ServiceProvider.GetRequiredService<IMultiLingualObjectManager>();

            var id = Guid.NewGuid();
            _book = new MultiLingualBook(id, "C# in Depth",100)
            {
                DefaultCulture = "en-US",
                Translations = new TranslationDictionary()
            };
            
            var tr = new MultiLingualBookTranslation
            {
                Culture = "tr-TR",
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

        [Fact]
        public async Task GetTranslationAsync()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book);

                translation.Name.ShouldBe("深入理解C#");
            }
        }

        [Fact]
        public async Task Should_Get_Specified_Language()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book, culture: "tr-TR");

                translation.Name.ShouldBe("C# Derinlemesine");
            }
        }
        
        [Fact]
        public async Task Should_Fallback_To_ParentCulture_And_Return_NULL_For_Default_Mapping()
        {
            using (CultureHelper.Use("tr-TR"))
            {
                var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book, culture: "us-GB");

                translation.ShouldBeNull();
            }
        }
    }
}
