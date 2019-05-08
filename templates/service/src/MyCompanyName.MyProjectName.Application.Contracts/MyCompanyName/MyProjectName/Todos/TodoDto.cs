using System;
using Volo.Abp.Application.Dtos;

namespace MyCompanyName.MyProjectName.Todos
{
    public class TodoDto : EntityDto<Guid>
    {
        public string Text { get; set; }
    }
}