using System;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging;

public class AuditLogEntityTypeFullNameConverter : ITransientDependency
{
    public virtual string Convert(string typeFullName)
    {
        var genericType = Regex.Match(typeFullName, @"(.+?)`1\[\[");
        if (!genericType.Success)
        {
            return ReplaceGenericSymbol(typeFullName);
        }

        var type = Regex.Match(typeFullName, @"`1\[\[(.+?), ");
        if (!type.Success)
        {
            return typeFullName;
        }

        if (type.Groups[1].Value.Contains("System.Nullable`1[["))
        {
            return genericType.Groups[1].Value + "<" + type.Groups[1].Value.Replace("System.Nullable`1[[", "") + "?>";
        }

        return genericType.Groups[1].Value.Contains("System.Nullable")
            ? type.Groups[1].Value + "?"
            : genericType.Groups[1].Value + "<" + ReplaceGenericSymbol(type.Groups[1].Value) + ">";
    }

    protected virtual string ReplaceGenericSymbol(string typeFullName)
    {
        return typeFullName.Contains("`1+")
            ? typeFullName.Substring(0, typeFullName.IndexOf("[[", StringComparison.Ordinal)).Replace("`1+", ".")
            : typeFullName;
    }
}
