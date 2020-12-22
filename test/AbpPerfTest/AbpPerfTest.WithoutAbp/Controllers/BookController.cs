using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpPerfTest.WithoutAbp.Dtos;
using AbpPerfTest.WithoutAbp.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbpPerfTest.WithoutAbp.Controllers
{
    [Route("api/books")]
    public class BookController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public BookController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        [HttpGet]
        public async Task<List<BookDto>> GetListAsync()
        {
            var books = await _bookDbContext.Books.ToListAsync();

            return books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    IsAvailable = b.IsAvailable
                })
                .ToList();
        }
    }
}
