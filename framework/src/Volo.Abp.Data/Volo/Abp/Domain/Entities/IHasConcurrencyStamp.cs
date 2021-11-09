namespace Volo.Abp.Domain.Entities;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}
