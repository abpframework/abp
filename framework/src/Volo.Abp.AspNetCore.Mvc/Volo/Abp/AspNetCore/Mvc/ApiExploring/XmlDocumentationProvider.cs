using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class XmlDocumentationProvider : IXmlDocumentationProvider, ISingletonDependency
{
    public ILogger<XmlDocumentationProvider> Logger { get; set; }

    public XmlDocumentationProvider()
    {
        Logger = NullLogger<XmlDocumentationProvider>.Instance;
    }

    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    // Matches any remaining XML tags like <c>, <code>, <para>, <b>, etc.
    private static readonly Regex XmlTagRegex = new(@"<[^>]+>", RegexOptions.Compiled);

    // Matches <see cref="T:Foo.Bar"/>, <see langword="null"/>, <paramref name="x"/>, <typeparamref name="T"/>
    private static readonly Regex XmlRefTagRegex = new(
        @"<(see|paramref|typeparamref)\s+(cref|name|langword)=""([TMFPE]:)?(?<display>[^""]+)""\s*/?>",
        RegexOptions.Compiled);

    private readonly ConcurrentDictionary<Assembly, Lazy<Task<XDocument?>>> _xmlDocCache = new();

    public virtual async Task<string?> GetSummaryAsync(Type type)
    {
        var memberName = GetMemberNameForType(type);
        return await GetDocumentationElementAsync(type.Assembly, memberName, "summary");
    }

    public virtual async Task<string?> GetRemarksAsync(Type type)
    {
        var memberName = GetMemberNameForType(type);
        return await GetDocumentationElementAsync(type.Assembly, memberName, "remarks");
    }

    public virtual async Task<string?> GetSummaryAsync(MethodInfo method)
    {
        var memberName = GetMemberNameForMethod(method);
        return await GetDocumentationElementAsync(method.DeclaringType!.Assembly, memberName, "summary");
    }

    public virtual async Task<string?> GetRemarksAsync(MethodInfo method)
    {
        var memberName = GetMemberNameForMethod(method);
        return await GetDocumentationElementAsync(method.DeclaringType!.Assembly, memberName, "remarks");
    }

    public virtual async Task<string?> GetReturnsAsync(MethodInfo method)
    {
        var memberName = GetMemberNameForMethod(method);
        return await GetDocumentationElementAsync(method.DeclaringType!.Assembly, memberName, "returns");
    }

    public virtual async Task<string?> GetParameterSummaryAsync(MethodInfo method, string parameterName)
    {
        var memberName = GetMemberNameForMethod(method);
        var doc = await LoadXmlDocumentationAsync(method.DeclaringType!.Assembly);
        if (doc == null)
        {
            return null;
        }

        var memberNode = doc.XPathSelectElement($"//member[@name='{memberName}']");
        var paramNode = memberNode?.XPathSelectElement($"param[@name='{parameterName}']");
        return CleanXmlText(paramNode);
    }

    public virtual async Task<string?> GetSummaryAsync(PropertyInfo property)
    {
        var memberName = GetMemberNameForProperty(property);
        return await GetDocumentationElementAsync(property.DeclaringType!.Assembly, memberName, "summary");
    }

    protected virtual async Task<string?> GetDocumentationElementAsync(Assembly assembly, string memberName, string elementName)
    {
        var doc = await LoadXmlDocumentationAsync(assembly);
        if (doc == null)
        {
            return null;
        }

        var memberNode = doc.XPathSelectElement($"//member[@name='{memberName}']");
        var element = memberNode?.Element(elementName);
        return CleanXmlText(element);
    }

    protected virtual Task<XDocument?> LoadXmlDocumentationAsync(Assembly assembly)
    {
        return _xmlDocCache.GetOrAdd(
            assembly,
            asm => new Lazy<Task<XDocument?>>(
                () => LoadXmlDocumentationFromDiskAsync(asm),
                LazyThreadSafetyMode.ExecutionAndPublication)
        ).Value;
    }

    protected virtual async Task<XDocument?> LoadXmlDocumentationFromDiskAsync(Assembly assembly)
    {
        if (string.IsNullOrEmpty(assembly.Location))
        {
            return null;
        }

        var xmlFilePath = Path.ChangeExtension(assembly.Location, ".xml");
        if (!File.Exists(xmlFilePath))
        {
            return null;
        }

        try
        {
            await using var stream = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to load XML documentation from {XmlFilePath}.", xmlFilePath);
            return null;
        }
    }

    private static string? CleanXmlText(XElement? element)
    {
        if (element == null)
        {
            return null;
        }

        // Convert to string first so we can process inline XML tags like <see cref="..."/>
        var raw = element.ToString();

        // Strip the outer element tags (e.g. <summary>...</summary>)
        var start = raw.IndexOf('>') + 1;
        var end = raw.LastIndexOf('<');
        if (start >= end)
        {
            return null;
        }

        var inner = raw[start..end];

        // Replace <see cref="T:Foo.Bar"/> with the short name "Bar"
        // Replace <see langword="null"/> with "null"
        // Replace <paramref name="x"/> and <typeparamref name="T"/> with the name
        inner = XmlRefTagRegex.Replace(inner, m =>
        {
            var display = m.Groups["display"].Value;
            // For cref values like "T:Foo.Bar.Baz", return only "Baz"
            var dot = display.LastIndexOf('.');
            return dot >= 0 ? display[(dot + 1)..] : display;
        });

        // Strip any remaining XML tags (e.g. <c>, <code>, <para>, <b>, etc.)
        inner = XmlTagRegex.Replace(inner, string.Empty);

        if (string.IsNullOrWhiteSpace(inner))
        {
            return null;
        }

        return WhitespaceRegex.Replace(inner.Trim(), " ");
    }

    private static string GetMemberNameForType(Type type)
    {
        return $"T:{GetTypeFullName(type)}";
    }

    private static string GetMemberNameForMethod(MethodInfo method)
    {
        var typeName = GetTypeFullName(method.DeclaringType!);
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
        {
            return $"M:{typeName}.{method.Name}";
        }

        var paramTypes = string.Join(",",
            parameters.Select(p => GetParameterTypeName(p.ParameterType)));
        return $"M:{typeName}.{method.Name}({paramTypes})";
    }

    private static string GetMemberNameForProperty(PropertyInfo property)
    {
        var typeName = GetTypeFullName(property.DeclaringType!);
        return $"P:{typeName}.{property.Name}";
    }

    private static string GetTypeFullName(Type type)
    {
        return type.FullName?.Replace('+', '.') ?? type.Name;
    }

    private static string GetParameterTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();
            var defName = genericDef.FullName!;
            defName = defName[..defName.IndexOf('`')];
            var args = string.Join(",", type.GetGenericArguments().Select(GetParameterTypeName));
            return $"{defName}{{{args}}}";
        }

        if (type.IsArray)
        {
            return GetParameterTypeName(type.GetElementType()!) + "[]";
        }

        if (type.IsByRef)
        {
            return GetParameterTypeName(type.GetElementType()!) + "@";
        }

        return type.FullName ?? type.Name;
    }
}
