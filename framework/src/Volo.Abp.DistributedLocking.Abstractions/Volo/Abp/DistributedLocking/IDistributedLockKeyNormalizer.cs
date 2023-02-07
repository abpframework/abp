namespace Volo.Abp.DistributedLocking;

public interface IDistributedLockKeyNormalizer
{
    string NormalizeKey(string name);
    
}