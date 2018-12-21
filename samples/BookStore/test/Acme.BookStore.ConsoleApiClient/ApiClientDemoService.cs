using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.ConsoleApiClient
{
    public class ApiClientDemoService : ITransientDependency
    {
        private readonly IBookAppService _bookAppService;

        public ApiClientDemoService(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }

        public async Task RunAsync()
        {
            var output = await _bookAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            foreach (var bookDto in output.Items)
            {
                Console.WriteLine($"[BOOK {bookDto.Id}] Name={bookDto.Name}, Price={bookDto.Price}");
            }
        }
    }
}
