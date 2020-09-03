using System.Collections.Generic;

namespace Volo.Abp.MultiLingualObject
{
    public interface IHasMultiLingual<TTranslation>
        where TTranslation : class, IMultiLingualTranslation
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}
