using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public static class AutoMapperExpressionExtensions
    {
        public static void Ignore<TDestination, TMember, TResult>(this IMappingExpression<TDestination, TMember> mappingExpression, Expression<Func<TMember, TResult>> destinationMember)
        {
            mappingExpression.ForMember(destinationMember, opts => opts.Ignore());
        }
    }
}
