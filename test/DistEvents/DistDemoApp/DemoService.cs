using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace DistDemoApp
{
    public class DemoService : ITransientDependency
    {
        private readonly IRepository<TodoItem, Guid> _todoItemRepository;

        public DemoService(IRepository<TodoItem, Guid> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        
        public async Task CreateTodoItemAsync()
        {
            var todoItem = await _todoItemRepository.InsertAsync(
                new TodoItem
                {
                    Text = "todo item " + DateTime.Now.Ticks
                }
            );
            
            Console.WriteLine("Created a new todo item: " + todoItem);
        }
    }
}