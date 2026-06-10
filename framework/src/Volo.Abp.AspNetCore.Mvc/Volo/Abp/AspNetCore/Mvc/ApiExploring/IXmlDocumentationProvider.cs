using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public interface IXmlDocumentationProvider
{
    Task<string?> GetSummaryAsync(Type type);

    Task<string?> GetRemarksAsync(Type type);

    Task<string?> GetSummaryAsync(MethodInfo method);

    Task<string?> GetRemarksAsync(MethodInfo method);

    Task<string?> GetReturnsAsync(MethodInfo method);

    Task<string?> GetParameterSummaryAsync(MethodInfo method, string parameterName);

    Task<string?> GetSummaryAsync(PropertyInfo property);
}
