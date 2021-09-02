using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace DistDemoApp
{
    public class TodoEventHandler :
        IDistributedEventHandler<EntityCreatedEto<TodoItemEto>>,
        IDistributedEventHandler<EntityDeletedEto<TodoItemEto>>,
        ITransientDependency
    {
        private readonly IRepository<TodoSummary, int> _todoSummaryRepository;

        public TodoEventHandler(IRepository<TodoSummary, int> todoSummaryRepository)
        {
            _todoSummaryRepository = todoSummaryRepository;
        }
        
        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEto<TodoItemEto> eventData)
        {
            var dateTime = eventData.Entity.CreationTime;
            var todoSummary = await _todoSummaryRepository.FindAsync(
                x => x.Year == dateTime.Year &&
                     x.Month == dateTime.Month &&
                     x.Day == dateTime.Day
            );

            if (todoSummary == null)
            {
                todoSummary = await _todoSummaryRepository.InsertAsync(new TodoSummary(dateTime));
            }
            else
            {
                todoSummary.Increase();
                await _todoSummaryRepository.UpdateAsync(todoSummary);
            }
            
            Console.WriteLine("Increased total count: " + todoSummary);

            throw new ApplicationException("Thrown to rollback the UOW!");
        }

        public async Task HandleEventAsync(EntityDeletedEto<TodoItemEto> eventData)
        {
            var dateTime = eventData.Entity.CreationTime;
            var todoSummary = await _todoSummaryRepository.FirstOrDefaultAsync(
                x => x.Year == dateTime.Year &&
                     x.Month == dateTime.Month &&
                     x.Day == dateTime.Day
            );

            if (todoSummary != null)
            {
                todoSummary.Decrease();
                await _todoSummaryRepository.UpdateAsync(todoSummary);
                Console.WriteLine("Decreased total count: " + todoSummary);
            }
        }
    }
}