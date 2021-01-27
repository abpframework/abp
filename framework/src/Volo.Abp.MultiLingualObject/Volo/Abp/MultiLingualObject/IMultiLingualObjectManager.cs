using System.Threading.Tasks;

namespace Volo.Abp.MultiLingualObject
{
    public interface IMultiLingualObjectManager
    {
        TTranslation GetTranslation<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            bool fallbackToParentCultures = true,
            string culture = null)
            where TMultiLingual : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation;

        Task<TTranslation> GetTranslationAsync<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            bool fallbackToParentCultures = true,
            string culture = null)
            where TMultiLingual : IHasMultiLingual<TTranslation>
            where TTranslation : class, IMultiLingualTranslation;
    }
}
