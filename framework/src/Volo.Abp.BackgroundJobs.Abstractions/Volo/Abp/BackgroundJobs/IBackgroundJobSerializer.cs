using System;

namespace Volo.Abp.BackgroundJobs
{
    public interface IBackgroundJobSerializer
    {
        string Serialize(object obj);

        object Deserialize(string value, Type type);
    }
}
