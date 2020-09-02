using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Settings;
using Volo.Abp.Testing;
using Xunit;

namespace AutoMapper
{
    public class AbpAutoMapperMultiLingualDtoExtensions_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly Volo.Abp.ObjectMapping.IObjectMapper _objectMapper;
        private readonly Book _book;

        public AbpAutoMapperMultiLingualDtoExtensions_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<Volo.Abp.ObjectMapping.IObjectMapper>();

            var id = Guid.NewGuid();
            _book = new Book(id, 100)
            {
                Translations = new List<BookTranslation>
                {
                    new BookTranslation
                    {
                        CoreId = id,
                        Language = "en",
                        Name = "C# in Depth"
                    },
                    new BookTranslation
                    {
                        CoreId = id,
                        Language = "zh-Hans",
                        Name = "深入理解C#"
                    }
                }
            };
        }

        [Fact]
        public void Should_Map_CurrentUICulture()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en");
            var bookDto = _objectMapper.Map<Book, BookDto>(_book);

            bookDto.Name.ShouldBe("C# in Depth");
            bookDto.Price.ShouldBe(_book.Price);
            bookDto.Id.ShouldBe(_book.Id);
        }

        [Fact]
        public void Should_Map_FeedbackUICulture()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-us");
            var bookDto = _objectMapper.Map<Book, BookDto>(_book);

            bookDto.Name.ShouldBe("C# in Depth");
            bookDto.Price.ShouldBe(100);
            bookDto.Id.ShouldBe(_book.Id);
        }
    }

    public class Book : Entity<Guid>, IMultiLingualEntity<BookTranslation>
    {
        public Book(Guid id, decimal price)
        {
            Id = id;
            Price = price;
        }

        public decimal Price { get; set; }

        public ICollection<BookTranslation> Translations { get; set; }
    }

    public class BookTranslation : IEntityTranslation<Book, Guid>
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public Book Core { get; set; }

        public Guid CoreId { get; set; }
    }

    public class BookDto
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public decimal Price { get; set; }
    }

    public class BookProfile : Profile
    {
        public BookProfile()
        {
        }

        public BookProfile(ISettingProvider settingsProvider)
        {
            this.CreateMultLingualMap<Book, BookTranslation, BookDto>(settingsProvider, true).TranslationMap
                .ForMember(d => d.Id, m => m.MapFrom(s => s.CoreId));
        }
    }
}
