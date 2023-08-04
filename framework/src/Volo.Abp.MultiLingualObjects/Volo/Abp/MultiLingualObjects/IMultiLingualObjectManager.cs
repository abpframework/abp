using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.MultiLingualObjects;

public interface IMultiLingualObjectManager
{
    Task<TTranslation?> GetTranslationAsync<TMultiLingual, TTranslation>(
        TMultiLingual multiLingual,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TMultiLingual : IMultiLingualObject<TTranslation>
        where TTranslation : class, IObjectTranslation;

    Task<TTranslation?> GetTranslationAsync<TTranslation>(
       IEnumerable<TTranslation> translations,
       string? culture = null,
       bool fallbackToParentCultures = true)
       where TTranslation : class, IObjectTranslation;


    Task<List<TTranslation?>> GetBulkTranslationsAsync<TTranslation>(
       IEnumerable<IEnumerable<TTranslation>> translationsCombined,
       string? culture = null,
       bool fallbackToParentCultures = true)
       where TTranslation : class, IObjectTranslation;

    Task<List<(TMultiLingual entity, TTranslation? translation)>> GetBulkTranslationsAsync<TMultiLingual, TTranslation>(
       IEnumerable<TMultiLingual> multiLinguals,
       string? culture = null,
       bool fallbackToParentCultures = true)
       where TMultiLingual : IMultiLingualObject<TTranslation>
       where TTranslation : class, IObjectTranslation;
}
