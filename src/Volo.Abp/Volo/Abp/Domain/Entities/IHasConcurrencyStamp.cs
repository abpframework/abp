namespace Volo.Abp.Domain.Entities
{
    //TODO: Think a better naming?
    public interface IHasConcurrencyStamp
    {
        string ConcurrencyStamp { get; set; }
    }
}