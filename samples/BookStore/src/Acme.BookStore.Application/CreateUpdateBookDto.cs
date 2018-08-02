using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AutoMapper;

namespace Acme.BookStore
{
    [AutoMapTo(typeof(Book))]
    [AutoMapFrom(typeof(BookDto))]
    public class CreateUpdateBookDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        
        public BookType Type { get; set; } = BookType.Undefined;
        
        public DateTime PublishDate { get; set; }
        
        public float Price { get; set; }
    }
}