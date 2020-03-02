using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Roles
{
    public class AppRoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Title { get; set; }
    }
}