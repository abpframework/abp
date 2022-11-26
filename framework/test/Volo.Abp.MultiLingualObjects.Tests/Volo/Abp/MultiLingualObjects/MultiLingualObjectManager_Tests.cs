using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.MultiLingualObjects.TestObjects;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.MultiLingualObjects;

public class MultiLingualObjectManager_Tests : AbpIntegratedTest<AbpMultiLingualObjectsTestModule>
{
    private readonly IMultiLingualObjectManager _multiLingualObjectManager;
    private readonly MultiLingualBook _book;
    private readonly List<MultiLingualBook> _books;
    private readonly IMapperAccessor _mapperAccessor;
    private readonly Dictionary<string, string> _testTranslations = new()
    {
        ["ar"] = "C# التعمق في",
        ["zh-Hans"] = "深入理解C#",
        ["en"] = "C# in Depth"
    };

    public MultiLingualObjectManager_Tests()
    {
        _multiLingualObjectManager = ServiceProvider.GetRequiredService<IMultiLingualObjectManager>();

        //Single Lookup
        _book = GetTestBook("en", "zh-Hans");
        //Bulk lookup
        _books = new List<MultiLingualBook>
        {
            //has no translations
            GetTestBook(),
            //english only
            GetTestBook("en"),
            //arabic only
            GetTestBook("ar"),
            //arabic + english
            GetTestBook("en","ar"),
            //arabic + english + chineese
            GetTestBook("en", "ar", "zh-Hans")
        };
        _mapperAccessor = ServiceProvider.GetRequiredService<IMapperAccessor>();
    }
    MultiLingualBook GetTestBook(params string[] included)
    {
        var id = Guid.NewGuid();
        //Single book
        var res = new MultiLingualBook(id, 100);

        foreach (var language in included)
        {
            res.Translations.Add(new MultiLingualBookTranslation
            {
                Language = language,
                Name = _testTranslations[language],
            });
        }

        return res;
    }

    [Fact]
    public async Task GetTranslationAsync()
    {
        using (CultureHelper.Use("en-us"))
        {
            var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book);
            translation.ShouldNotBeNull();
            translation.Name.ShouldBe(_testTranslations["en"]);
        }
    }

    [Fact]
    public async Task GetTranslationFromListAsync()
    {
        using (CultureHelper.Use("en-us"))
        {
            var translation = await _multiLingualObjectManager.GetTranslationAsync(_book.Translations);
            translation.ShouldNotBeNull();
            translation.Name.ShouldBe(_testTranslations["en"]);
        }
    }

    [Fact]
    public async Task Should_Get_Specified_Language()
    {
        using (CultureHelper.Use("zh-Hans"))
        {
            var translation = await _multiLingualObjectManager.GetTranslationAsync<MultiLingualBook, MultiLingualBookTranslation>(_book, culture: "en");
            translation.ShouldNotBeNull();
            translation.Name.ShouldBe(_testTranslations["en"]);
        }
    }


    [Fact]
    public async Task GetBulkTranslationsAsync()
    {
        using (CultureHelper.Use("en-us"))
        {
            var translations = await _multiLingualObjectManager.GetBulkTranslationsAsync<MultiLingualBook, MultiLingualBookTranslation>(_books);
            foreach (var (entity, translation) in translations)
            {
                if (entity.Translations.Any(x => x.Language == "en"))
                {
                    translation.ShouldNotBeNull();
                    translation.Name.ShouldBe(_testTranslations["en"]);
                }
                else
                {
                    translation.ShouldBeNull();
                }
            }
        }
    }

    [Fact]
    public async Task GetBulkTranslationsFromListAsync()
    {
        using (CultureHelper.Use("en-us"))
        {
            var translations = await _multiLingualObjectManager.GetBulkTranslationsAsync(_books.Select(x => x.Translations));
            foreach (var translation in translations)
            {
                translation?.Name.ShouldBe(_testTranslations["en"]);
            }
        }
    }

    [Fact]
    public async Task TestBulkMapping()
    {
        using (CultureHelper.Use("en-us"))
        {
            var translations = await _multiLingualObjectManager.GetBulkTranslationsAsync<MultiLingualBook, MultiLingualBookTranslation>(_books);
            var translationsDict = translations.ToDictionary(x => x.entity.Id, x => x.translation);
            var mapped = _mapperAccessor.Mapper.Map<List<MultiLingualBook>, List<MultiLingualBookDto>>(_books, options =>
            {
                options.Items.Add(nameof(MultiLingualBookTranslation), translationsDict);
            });
            Assert.Equal(mapped.Count, _books.Count);
            for (int i = 0; i < mapped.Count; i++)
            {
                var og = _books[i];
                var m = mapped[i];
                Assert.Equal(og.Translations.FirstOrDefault(x => x.Language == "en")?.Name, m.Name);
            }
        }
    }
}
