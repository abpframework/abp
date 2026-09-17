using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Volo.Abp.Validation;

[assembly: InternalsVisibleTo("Volo.Abp.Ddd.Application.Tests")]

namespace Volo.Abp.Application.Services;

/// <summary>
/// Framework infrastructure. Hooks <see cref="ExtensibilityPoint.QueryOptimizer"/> so
/// every OrderBy / ThenBy expression built from a user-supplied sorting string is
/// constrained to plain property or field access. Methods, comparisons, ternaries
/// and constants in the sort key are rejected with <see cref="AbpValidationException"/>.
/// Property-bag / shadow-property access through a constant string indexer
/// (e.g. <c>it["Prop"]</c>, <c>Data["Prop"]</c>) is treated like plain property
/// access and allowed, because it is the canonical way to sort dynamically-mapped
/// entities. The guard assumes the matching indexer getter behaves like a property
/// getter; defining an indexer that performs IO or mutates state will expose those
/// effects through sorting.
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
        {
            // Allow property-bag / shadow-property access through a constant string
            // indexer (it["Prop"], Data["Prop"], mainEntity["Prop"], ...). The
            // constant key cannot smuggle in an arbitrary method invocation, so this
            // does not widen the injection surface the guard protects against; it
            // treats the indexer access like ordinary property access.
            if (IsConstantStringIndexer(node))
            {
                if (node.Object != null)
                {
                    Visit(node.Object);
                }

                return node;
            }

            throw new AbpValidationException(Message);
        }

        protected override Expression VisitBinary(BinaryExpression node)
            => throw new AbpValidationException(Message);

        protected override Expression VisitConditional(ConditionalExpression node)
            => throw new AbpValidationException(Message);

        protected override Expression VisitConstant(ConstantExpression node)
            => throw new AbpValidationException(Message);

        private static bool IsConstantStringIndexer(MethodCallExpression node)
        {
            // Must be an instance call with a single constant string argument.
            if (node.Object == null
                || node.Arguments.Count != 1
                || node.Arguments[0] is not ConstantExpression { Value: string })
            {
                return false;
            }

            // And the resolved method must be the get accessor of a real string-keyed
            // indexer (a special-name property getter), not an arbitrary method that
            // merely shares the compiler-generated "get_Item" name. This keeps the
            // guard's "no method calls" rule intact for everything except indexers.
            var method = node.Method;
            var parameters = method.GetParameters();
            if (!method.IsSpecialName
                || method.IsStatic
                || parameters.Length != 1
                || parameters[0].ParameterType != typeof(string))
            {
                return false;
            }

            return method.DeclaringType?
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(property =>
                {
                    var indexParameters = property.GetIndexParameters();
                    return indexParameters.Length == 1
                        && indexParameters[0].ParameterType == typeof(string)
                        && property.GetGetMethod(nonPublic: true) == method;
                }) == true;
        }
    }
}
