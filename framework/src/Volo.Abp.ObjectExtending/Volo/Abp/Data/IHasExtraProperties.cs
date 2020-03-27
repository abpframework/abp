using System.Collections.Generic;

namespace Volo.Abp.Data
{
    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}
