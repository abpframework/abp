namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public interface IMemoryDbSerializer
    {
        object Serialize(object obj);

        object Deserialize(object obj);
    }
}