using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Pages.Books
{
    public class CreateModalModel : AbpPageModel
    {
        [BindProperty]
        public CreateBookViewModel Book { get; set; }

        private readonly IBookAppService _bookAppService;

        public CreateModalModel(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var bookDto = ObjectMapper.Map<CreateBookViewModel, BookDto>(Book);
            await _bookAppService.CreateAsync(bookDto);

            return NoContent();
        }

        public class CreateBookViewModel
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
}