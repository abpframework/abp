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
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public BookType Type { get; set; } = BookType.Undefined;

        [Display(Name = "PublishDate")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Price")]
        public float Price { get; set; }
    }
}