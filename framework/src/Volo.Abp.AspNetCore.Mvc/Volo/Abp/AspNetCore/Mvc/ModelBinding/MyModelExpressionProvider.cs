using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Volo.Abp.AspNetCore.Mvc.ModelBinding.Metadata;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    [Dependency(ReplaceServices = true)]
    public class AbpModelExpressionProvider : IModelExpressionProvider, ITransientDependency
    {
        private readonly ModelExpressionProvider _modelExpressionProvider;

        public AbpModelExpressionProvider(ModelExpressionProvider modelExpressionProvider)
        {
            _modelExpressionProvider = modelExpressionProvider;
        }

        public ModelExpression CreateModelExpression<TModel, TValue>(
            ViewDataDictionary<TModel> viewData,
            Expression<Func<TModel, TValue>> expression)
        {
            var result = _modelExpressionProvider.CreateModelExpression(
                viewData,
                expression
            );

            if (result.ModelExplorer.Container == null)
            {
                return result;
            }

            var extraPropertyName = ExtraPropertyBindingHelper.ExtractExtraPropertyName(result.Name);
            if (extraPropertyName == null)
            {
                return result;
            }

            var containerName = ExtraPropertyBindingHelper.ExtractContainerName(result.Name);
            if (containerName == null)
            {
                return result;
            }

            var containerModelExpolorer = result
                .ModelExplorer
                .Container
                .Properties
                .FirstOrDefault(p => p.Metadata.Name == containerName);

            if (containerModelExpolorer == null)
            {
                return result;
            }

            if (!containerModelExpolorer.ModelType.IsAssignableTo<IHasExtraProperties>())
            {
                return result;
            }

            AbpExtraPropertyValidationMetadataProvider.CurrentModeInfo.Value
                = new AbpExtraPropertyValidationMetadataProvider.ExtraPropertyModelInfo(
                    containerModelExpolorer.ModelType,
                    extraPropertyName
                );

            return result;
        }
    }
}