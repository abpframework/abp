using System.Collections.Generic;

namespace Volo.Abp.Data
{
    //TODO: Move to Volo.Abp.Data.ObjectExtending namespace at v3.0

    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}
