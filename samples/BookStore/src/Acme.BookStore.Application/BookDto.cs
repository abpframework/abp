using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;

namespace Acme.BookStore
{
    [AutoMapFrom(typeof(Book))]
    public class BookDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}