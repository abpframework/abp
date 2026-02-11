using System;
using System.Linq.Expressions;

namespace Volo.Abp.EventBus.Distributed;

public static class InboxOutboxFilterExpressionTransformer
{
    public static Expression<Func<TTarget, bool>> Transform<TOriginal, TTarget>(Expression<Func<TOriginal, bool>> originalExpression)
    {
        var originalParam = originalExpression.Parameters[0];
        var newParam = Expression.Parameter(typeof(TTarget), originalParam.Name);
        var body = ReplaceParameter(originalExpression.Body, originalParam, newParam);
        return Expression.Lambda<Func<TTarget, bool>>(body, newParam);
    }

    private static Expression ReplaceParameter(Expression body, ParameterExpression oldParam, ParameterExpression newParam)
    {
        var visitor = new ParameterReplacer(oldParam, newParam);
        return visitor.Visit(body);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParam;
        private readonly ParameterExpression _newParam;

        public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
        {
            _oldParam = oldParam;
            _newParam = newParam;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParam ? _newParam : base.VisitParameter(node);
        }
    }
}
