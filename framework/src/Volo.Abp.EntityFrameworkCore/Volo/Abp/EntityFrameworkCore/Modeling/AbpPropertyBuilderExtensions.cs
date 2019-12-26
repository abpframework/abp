using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Volo.Abp.EntityFrameworkCore.Modeling
{
    public static class AbpPropertyBuilderExtensions
    {
        public static void ConfigureJsonConversionWithValueComparer<TPropertyType>(
            this PropertyBuilder<TPropertyType> propertyBuilder,
            Expression<Func<TPropertyType, TPropertyType, bool>> equalsExpression,
            Expression<Func<TPropertyType, int>> hashCodeExpression,
            Expression<Func<TPropertyType, TPropertyType>> snapshotExpression)
        {
            propertyBuilder.HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<TPropertyType>(s)
                )
                .Metadata.SetValueComparer(new ValueComparer<TPropertyType>(
                    equalsExpression,
                    hashCodeExpression,
                    snapshotExpression));
        }
    }
}