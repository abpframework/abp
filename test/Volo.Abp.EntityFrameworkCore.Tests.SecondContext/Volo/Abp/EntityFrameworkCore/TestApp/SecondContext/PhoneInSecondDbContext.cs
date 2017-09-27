using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    [Table("AppPhones")]
    public class PhoneInSecondDbContext : AggregateRoot<long>
    {
        public virtual string Number { get; set; }
    }
}