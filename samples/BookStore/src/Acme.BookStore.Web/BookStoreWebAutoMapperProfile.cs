using Acme.BookStore.Pages.Books;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Acme.BookStore
{
    public class BookStoreWebAutoMapperProfile : Profile
    {
        public BookStoreWebAutoMapperProfile()
        {
            CreateMap<CreateModalModel.CreateBookViewModel, BookDto>()
                .Ignore(x => x.Id);
        }
    }
}
