using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
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

        [Fact]
        public async Task GetTranslationAsync()
        {
            using (CultureHelper.Use("en-us"))
            {
                var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book);

                translation.Name.ShouldBe("C# in Depth");
            }
        }

        [Fact]
        public async Task Should_Get_Specified_Language()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book, culture: "en");

                translation.Name.ShouldBe("C# in Depth");
            }
        }
    }
}
