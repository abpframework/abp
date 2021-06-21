using System.Threading.Tasks;

namespace Volo.Abp.MultiLingualObjects
{
    public interface IMultiLingualObjectManager
    {
        Task<TTranslation> GetTranslationAsync<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            string culture = null,
            bool fallbackToParentCultures = true)
            where TMultiLingual : IMultiLingualObject<TTranslation>
            where TTranslation : class, IObjectTranslation;
    }
}
