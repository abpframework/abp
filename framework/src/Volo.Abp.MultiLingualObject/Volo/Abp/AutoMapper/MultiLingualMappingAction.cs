using AutoMapper;
using Volo.Abp.MultiLingualObject;

namespace Volo.Abp.AutoMapper
{
    public class MultiLingualMappingAction<TMultiLingualEntity, TMultiLingualEntityPrimaryKey, TTranslation, TDestination> : IMappingAction<TMultiLingualEntity, TDestination>
         where TTranslation : class, IMultiLingualTranslation<TMultiLingualEntity, TMultiLingualEntityPrimaryKey>
         where TMultiLingualEntity : class, IHasMultiLingual<TTranslation>
    {
        private readonly IMultiLingualObjectManager _objectManager;

        public MultiLingualMappingAction(IMultiLingualObjectManager objectManager)
        {
            _objectManager = objectManager;
        }

        public void Process(TMultiLingualEntity source, TDestination destination, ResolutionContext context)
        {
            var translation = _objectManager.GetTranslationAsync<TMultiLingualEntity, TTranslation>(source, false).Result;

            if (translation != null)
            {
                context.Mapper.Map(translation, destination);
            }
        }
    }
}
