using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.Blogging.Posts
{
    public class BlogUserDto : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        
        public string WebSite { get; set; }

        public string Twitter { get; set; }

        public string Github { get; set; }

        public string Linkedin { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }
        
        public string Biography { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}
