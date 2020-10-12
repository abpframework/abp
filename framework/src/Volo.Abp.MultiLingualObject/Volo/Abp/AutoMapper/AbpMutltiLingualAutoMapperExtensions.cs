using AutoMapper;
using System;
using Volo.Abp.MultiLingualObject;

namespace Volo.Abp.AutoMapper
{
    public static class AbpMutltiLingualAutoMapperExtensions
    {
        public static CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination> CreateMultiLingualMap<TMultiLingualEntity, TMultiLingualEntityPrimaryKey, TTranslation, TDestination>(
            this IProfileExpression configuration, bool fallbackToParentCultures = false)
            where TTranslation : class, IMultiLingualTranslation<TMultiLingualEntity, TMultiLingualEntityPrimaryKey>
            where TMultiLingualEntity : class, IHasMultiLingual<TTranslation>
        {
            var result = new CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination>();

            result.TranslationMap = configuration.CreateMap<TTranslation, TDestination>();

            if (fallbackToParentCultures)
            {
                result.EntityMap = configuration
                    .CreateMap<TMultiLingualEntity, TDestination>()
                    .BeforeMap<RecursiveMultiLingualMappingAction<TMultiLingualEntity, TMultiLingualEntityPrimaryKey, TTranslation, TDestination>>();
            }
            else
            {
                result.EntityMap = configuration
                    .CreateMap<TMultiLingualEntity, TDestination>()
                    .BeforeMap<MultiLingualMappingAction<TMultiLingualEntity, TMultiLingualEntityPrimaryKey, TTranslation, TDestination>>();
            }

            return result;
        }

        public static CreateMultiLingualMapResult<TMultiLingualEntity, TTranslation, TDestination> CreateMultiLingualMap<TMultiLingualEntity, TTranslation, TDestination>(
            this IProfileExpression configuration,
            bool fallbackToParentCultures = false)
            where TTranslation : class, IMultiLingualTranslation<TMultiLingualEntity, Guid>
            where TMultiLingualEntity : class, IHasMultiLingual<TTranslation>
        {
            return configuration.CreateMultiLingualMap<TMultiLingualEntity, Guid, TTranslation, TDestination>(fallbackToParentCultures);
        }
    }
}
