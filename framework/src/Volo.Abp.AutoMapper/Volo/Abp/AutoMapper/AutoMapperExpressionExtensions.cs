using System;
using System.Linq.Expressions;
using AutoMapper;
using Volo.Abp.Auditing;

namespace Volo.Abp.AutoMapper
{
    public static class AutoMapperExpressionExtensions
    {
        public static IMappingExpression<TDestination, TMember> Ignore<TDestination, TMember, TResult>(this IMappingExpression<TDestination, TMember> mappingExpression, Expression<Func<TMember, TResult>> destinationMember)
        {
            return mappingExpression.ForMember(destinationMember, opts => opts.Ignore());
        }
        
        public static IMappingExpression<TSource, TDestination> IgnoreCreationTime<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
            where TDestination : IHasCreationTime
        {
            return mappingExpression.Ignore(x => x.CreationTime);
        }
    }
}
