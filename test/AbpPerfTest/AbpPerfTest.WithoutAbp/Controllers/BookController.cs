using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpPerfTest.WithoutAbp.Dtos;
using AbpPerfTest.WithoutAbp.Entities;
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
            var books = await _bookDbContext.Books.OrderBy(x => x.Id).Take(10).ToListAsync();

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

        [HttpGet]
        [Route("{id}")]
        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookDbContext.Books.SingleAsync(b => b.Id == id);

            return new BookDto
            {
                Id = book.Id,
                Name = book.Name,
                Price = book.Price,
                IsAvailable = book.IsAvailable
            };
        }

        [HttpPost]
        public async Task<Guid> CreateAsync([FromBody] CreateUpdateBookDto input)
        {
            var book = new Book
            {
                Name = input.Name,
                Price = input.Price,
                IsAvailable = input.IsAvailable
            };

            await _bookDbContext.Books.AddAsync(book);
            await _bookDbContext.SaveChangesAsync();

            return book.Id;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task UpdateAsync(Guid id, [FromBody] CreateUpdateBookDto input)
        {
            var book = await _bookDbContext.Books.SingleAsync(b => b.Id == id);

            book.Name = input.Name;
            book.Price = input.Price;
            book.IsAvailable = input.IsAvailable;

            await _bookDbContext.SaveChangesAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookDbContext.Books.SingleAsync(b => b.Id == id);

            _bookDbContext.Books.Remove(book);
            await _bookDbContext.SaveChangesAsync();
        }
    }
}
