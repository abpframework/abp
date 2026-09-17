using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering;

internal static class FilterExpressionPropertyNameInspector
{
    // EF Core stores the registered query filter under the CoreAnnotationNames.QueryFilter annotation.
    // Since EF Core 9 the value is a QueryFilterCollection (internal) whose elements expose an
    // Expression property. Older versions stored a bare LambdaExpression. We handle both via duck typing
    // so the test stays compatible across EF Core versions.
    public static List<string> GetEfPropertyStringArgs(object annotationValue)
    {
        return Inspect(annotationValue).Args;
    }

    public static InspectionResult Inspect(object annotationValue)
    {
        var collector = new EfPropertyArgCollector();
        var dump = new StringBuilder();
        dump.Append("annotationType=").Append(annotationValue?.GetType().FullName ?? "<null>").AppendLine();

        switch (annotationValue)
        {
            case LambdaExpression lambda:
                dump.Append("lambda: ").AppendLine(lambda.ToString());
                collector.Visit(lambda);
                break;
            case IEnumerable enumerable:
                var index = 0;
                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    dump.Append("[").Append(index++).Append("] itemType=").Append(item.GetType().FullName).AppendLine();
                    var expr = item.GetType().GetProperty("Expression")?.GetValue(item) as Expression
                               ?? item as Expression;
                    if (expr != null)
                    {
                        dump.Append("    expr: ").AppendLine(expr.ToString());
                        collector.Visit(expr);
                    }
                    else
                    {
                        dump.AppendLine("    (no Expression property)");
                    }
                }
                break;
        }

        return new InspectionResult(collector.Args, dump.ToString());
    }

    public sealed record InspectionResult(List<string> Args, string Dump);

    private sealed class EfPropertyArgCollector : ExpressionVisitor
    {
        public List<string> Args { get; } = new();

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(EF)
                && node.Method.Name == nameof(EF.Property)
                && node.Arguments.Count == 2)
            {
                // The 2nd arg comes from a closure-captured local at filter registration, so it
                // appears as a MemberExpression rather than a ConstantExpression. Evaluate it.
                if (Expression.Lambda(node.Arguments[1]).Compile().DynamicInvoke() is string name)
                {
                    Args.Add(name);
                }
            }

            return base.VisitMethodCall(node);
        }
    }
}
