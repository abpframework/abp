using System.Collections.Generic;

namespace Volo.Abp.MultiLingualEntities
{
    public interface IMultiLingualEntity<TTranslation>
        where TTranslation : class, IEntityTranslation
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}
