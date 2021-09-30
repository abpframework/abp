using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace DistDemoApp
{
    public class TodoItemObjectMapper : IObjectMapper<TodoItem, TodoItemEto>, ISingletonDependency
    {
        public TodoItemEto Map(TodoItem source)
        {
            return new TodoItemEto
            {
                Text = source.Text,
                CreationTime = source.CreationTime
            };
        }

        public TodoItemEto Map(TodoItem source, TodoItemEto destination)
        {
            destination.Text = source.Text;
            destination.CreationTime = source.CreationTime;
            return destination;
        }
    }
}