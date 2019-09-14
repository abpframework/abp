using Acme.BookStore.BookManagement.Books;
using AutoMapper;

namespace Acme.BookStore.BookManagement
{
    public class BookManagementApplicationAutoMapperProfile : Profile
    {
        public BookManagementApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>(MemberList.Source);
        }
    }
}