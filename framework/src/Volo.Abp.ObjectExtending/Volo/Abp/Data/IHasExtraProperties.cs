using System.Collections.Generic;

namespace Volo.Abp.Data
{
    //TODO: Move to Volo.Abp.Data.ObjectExtending namespace at 4.0?

    public interface IHasExtraProperties
    {
        ExtraPropertyDictionary ExtraProperties { get; }
    }

    public class ExtraPropertyDictionary : Dictionary<string, object>
    {
        public ExtraPropertyDictionary()
        {
        }

        public ExtraPropertyDictionary(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }
    }
}
