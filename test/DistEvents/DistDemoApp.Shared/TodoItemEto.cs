using System;
using Volo.Abp.EventBus;

namespace DistDemoApp
{
    [EventName("todo-item")]
    public class TodoItemEto
    {
        public DateTime CreationTime { get; set; }
        public string Text { get; set; }
    }
}