using Volo.Abp.MultiLingualObject;
using Volo.Abp.Threading;

namespace AutoMapper
{
    public static class AbpAutoMapperMultiLingualDtoExtensions
    {
        public static CreateMultiLingualMapResult<TSource, TTranslation, TDestination> CreateMultiLingualMap<TSource, TTranslation, TDestination>(this Profile profile)
            where TTranslation : class, IMultiLingualTranslation
            where TSource : IHasMultiLingual<TTranslation>
        {

            return new(
                profile.CreateMap<TSource, TDestination>().BeforeMap<AbpMultiLingualMapperAction<TSource, TTranslation, TDestination>>(),
                profile.CreateMap<TTranslation, TDestination>());
        }
    }

    public class AbpMultiLingualMapperAction<TSource, TTranslation, TDestination> : IMappingAction<TSource, TDestination>
        where TTranslation : class, IMultiLingualTranslation
        where TSource : IHasMultiLingual<TTranslation>
    {
        private readonly IMultiLingualObjectManager _multiLingualObjectManager;

        public AbpMultiLingualMapperAction(IMultiLingualObjectManager multiLingualObjectManager)
        {
            _multiLingualObjectManager = multiLingualObjectManager;
        }

        public void Process(TSource source, TDestination destination, ResolutionContext context)
        {
            var translation = AsyncHelper.RunSync(() => _multiLingualObjectManager.GetTranslationAsync<TSource, TTranslation>(source));
            if (translation != null)
            {
                context.Mapper.Map(translation, destination);
            }
        }
    }

    public class CreateMultiLingualMapResult<TSource, TTranslation, TDestination>
    {
        public IMappingExpression<TSource, TDestination> EntityMap { get; }

        public IMappingExpression<TTranslation, TDestination> TranslateMap { get; }

        public CreateMultiLingualMapResult(
            IMappingExpression<TSource, TDestination> entityMap,
            IMappingExpression<TTranslation, TDestination> translateMap)
        {
            EntityMap = entityMap;
            TranslateMap = translateMap;
        }
    }
}
