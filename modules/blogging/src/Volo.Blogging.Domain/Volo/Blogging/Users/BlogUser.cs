using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace Volo.Blogging.Users
{
    public class BlogUser : AggregateRoot<Guid>, IUser, IHasExtraProperties
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual string UserName { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual bool EmailConfirmed { get; protected set; }

        public virtual string PhoneNumber { get; protected set; }

        public virtual bool PhoneNumberConfirmed { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        protected BlogUser()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public BlogUser(IUserData user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            EmailConfirmed = user.EmailConfirmed;
            PhoneNumber = user.PhoneNumber;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            UserName = user.UserName;
            TenantId = user.TenantId;

            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
