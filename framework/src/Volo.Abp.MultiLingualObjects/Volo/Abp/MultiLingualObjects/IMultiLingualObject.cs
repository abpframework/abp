using System.Collections.Generic;

namespace Volo.Abp.MultiLingualObjects;

public interface IMultiLingualObject<TTranslation>
    where TTranslation : class, IObjectTranslation
{
    ICollection<TTranslation> Translations { get; set; }
}
