using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MongoDB.TestApp.SecondContext
{
    public class PhoneInSecondDbContext : AggregateRoot
    {
        public virtual Guid PersonId { get; set; }

        public virtual string Number { get; set; }

        public override object[] GetKeys()
        {
            return new object[] {PersonId, Number};
        }
    }
}
