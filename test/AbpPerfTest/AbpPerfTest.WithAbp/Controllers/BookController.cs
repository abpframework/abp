using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpPerfTest.WithAbp.Dtos;
using AbpPerfTest.WithAbp.Entities;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace AbpPerfTest.WithAbp.Controllers
{
    [Route("api/books")]
    public class BookController : Controller
    {
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookController(IRepository<Book, Guid> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<List<BookDto>> GetListAsync()
        {
            var books = await _bookRepository.GetPagedListAsync(0, 10, "Id");

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
            var book = await _bookRepository.GetAsync(id);

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

            await _bookRepository.InsertAsync(book);

            return book.Id;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task UpdateAsync(Guid id, [FromBody] CreateUpdateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            book.Name = input.Name;
            book.Price = input.Price;
            book.IsAvailable = input.IsAvailable;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
