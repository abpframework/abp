using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Instrumentation.Sentry;

public static class SentryHelper
{
    public static bool IsSentrySpanType(TypeInfo implementationType)
    {
        //Explicitly defined SentrySpanAttribute
        if (HasSentryAttribute(implementationType) || AnyMethodHasSentrySpanAttribute(implementationType))
        {
            return true;
        }

        //Conventional classes
        if (typeof(ISentrySpanEnabled).GetTypeInfo().IsAssignableFrom(implementationType))
        {
            return true;
        }

        return false;
    }


    public static bool IsSentrySpanMethod(MethodInfo methodInfo,
        out SentrySpanAttribute? sentrySpanAttribute)
    {
        Check.NotNull(methodInfo, nameof(methodInfo));

        //Method declaration
        var attrs = methodInfo.GetCustomAttributes(true).OfType<SentrySpanAttribute>().ToArray();
        if (attrs.Any())
        {
            sentrySpanAttribute = attrs.First();
            return !sentrySpanAttribute.IsDisabled;
        }

        if (methodInfo.DeclaringType != null)
        {
            //Class declaration
            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<SentrySpanAttribute>()
                .ToArray();
            if (attrs.Any())
            {
                sentrySpanAttribute = attrs.First();
                return !sentrySpanAttribute.IsDisabled;
            }

            //Conventional classes
            if (typeof(ISentrySpanEnabled).GetTypeInfo().IsAssignableFrom(methodInfo.DeclaringType))
            {
                sentrySpanAttribute = null;
                return true;
            }
        }

        sentrySpanAttribute = null;
        return false;
    }

    private static bool AnyMethodHasSentrySpanAttribute(TypeInfo implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasSentryAttribute);
    }

    private static bool HasSentryAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(SentrySpanAttribute), true);
    }
}