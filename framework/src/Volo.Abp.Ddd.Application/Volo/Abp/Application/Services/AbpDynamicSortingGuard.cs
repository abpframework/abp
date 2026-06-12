using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Volo.Abp.Validation;

[assembly: InternalsVisibleTo("Volo.Abp.Ddd.Application.Tests")]

namespace Volo.Abp.Application.Services;

/// <summary>
/// Framework infrastructure. Hooks <see cref="ExtensibilityPoint.QueryOptimizer"/> so
/// every OrderBy / ThenBy expression built from a user-supplied sorting string is
/// constrained to plain property or field access. Methods, comparisons, ternaries
/// and constants in the sort key are rejected with <see cref="AbpValidationException"/>.
/// </summary>
internal static class AbpDynamicSortingGuard
{
    private static readonly object InstallLock = new();
    private static Func<Expression, Expression>? _activeOptimizer;

    public static void Install()
    {
        lock (InstallLock)
        {
            var current = ExtensibilityPoint.QueryOptimizer;
            if (_activeOptimizer != null && ReferenceEquals(current, _activeOptimizer))
            {
                return;
            }

            var previous = current;
            _activeOptimizer = expression =>
            {
                new OrderByMethodVisitor().Visit(expression);
                return previous != null ? previous(expression) : expression;
            };
            ExtensibilityPoint.QueryOptimizer = _activeOptimizer;
        }
    }

    internal static void Reset()
    {
        lock (InstallLock)
        {
            if (ReferenceEquals(ExtensibilityPoint.QueryOptimizer, _activeOptimizer))
            {
                ExtensibilityPoint.QueryOptimizer = null;
            }
            _activeOptimizer = null;
        }
    }

    private sealed class OrderByMethodVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) &&
                IsOrderByMethod(node.Method.Name) &&
                node.Arguments.Count >= 2 &&
                node.Arguments[1] is UnaryExpression { Operand: LambdaExpression lambda })
            {
                new PropertyOnlySelectorVisitor().Visit(lambda.Body);
            }

            return base.VisitMethodCall(node);
        }

        private static bool IsOrderByMethod(string name)
        {
            return name == nameof(Queryable.OrderBy)
                || name == nameof(Queryable.OrderByDescending)
                || name == nameof(Queryable.ThenBy)
                || name == nameof(Queryable.ThenByDescending);
        }
    }

    private sealed class PropertyOnlySelectorVisitor : ExpressionVisitor
    {
        private const string Message = "Sorting expression is not supported.";

        protected override Expression VisitMethodCall(MethodCallExpression node)
            => throw new AbpValidationException(Message);

        protected override Expression VisitBinary(BinaryExpression node)
            => throw new AbpValidationException(Message);

        protected override Expression VisitConditional(ConditionalExpression node)
            => throw new AbpValidationException(Message);

        protected override Expression VisitConstant(ConstantExpression node)
            => throw new AbpValidationException(Message);
    }
}
