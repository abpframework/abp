using System;
using System.Linq.Expressions;

namespace Volo.Abp.EntityFrameworkCore;

public static class QueryFilterExpressionHelper
{
    public static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expression1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expression2.Body);

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left!, right!), parameter);
    }

    /// <summary>
    /// Lifts a filter of a derived entity type to its base type, so that it can be set on the root of the hierarchy.
    /// Entities that are not of the derived type are not affected: <c>e => !(e is TDerived) || filter((TDerived)e)</c>.
    /// </summary>
    public static Expression<Func<T, bool>> ApplyToDerivedType<T, TDerived>(Expression<Func<TDerived, bool>> expression)
        where TDerived : T
    {
        var parameter = Expression.Parameter(typeof(T));

        var visitor = new ReplaceExpressionVisitor(expression.Parameters[0], Expression.Convert(parameter, typeof(TDerived)));
        var body = visitor.Visit(expression.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(Expression.Not(Expression.TypeIs(parameter, typeof(TDerived))), body!),
            parameter
        );
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression? Visit(Expression? node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
