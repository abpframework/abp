using System.Text.Json;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;

// Validates the Scriban template syntax embedded in `docs/en/` Markdown files.
//
// For each input file we run `Template.Parse` and a strict-mode render with the
// same parameter set the docs renderer injects at runtime (each docs-params.json
// `<name>` and its `<name>_Value` companion, plus Document_Language_Code,
// Document_Version and Release_Status). StrictVariables is enabled on purpose so
// references that would otherwise be silently rendered as empty strings surface
// as build failures here.
//
// Known limitations:
// - Partial template inlining is not executed: partial bodies are loaded from
//   external storage at render time, so they cannot be resolved in CI. Files
//   under `docs/en/` currently have no `//[doc-template]` references; if one is
//   added later, errors inside the partial body must be reviewed manually.
// - Cookie- and query-string-driven parameter overrides are not injected, but
//   their keys still resolve to empty strings because they layer on top of the
//   same `<name>` / `<name>_Value` entries that are already injected.

namespace Volo.Abp.Docs.SyntaxCheck;

internal static class Program
{
    private const string DefaultDocsRoot = "docs/en";
    private const string DocsParamsFileName = "docs-params.json";

    private static readonly string[] BuiltInVariables =
    {
        "Document_Language_Code",
        "Document_Version",
        "Release_Status"
    };

    public static int Main(string[] args)
    {
        var useGitHubAnnotations = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";

        var inputPaths = args.Length == 0
            ? new[] { DefaultDocsRoot }
            : args;

        var files = new List<string>();
        foreach (var path in inputPaths)
        {
            if (File.Exists(path))
            {
                if (path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(Path.GetFullPath(path));
                }
            }
            else if (Directory.Exists(path))
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.md", SearchOption.AllDirectories))
                {
                    files.Add(Path.GetFullPath(file));
                }
            }
            else
            {
                Console.Error.WriteLine($"WARN: path does not exist: {path}");
            }
        }

        if (files.Count == 0)
        {
            Console.WriteLine("No markdown files to check.");
            return 0;
        }

        Dictionary<string, string> renderParameters;
        try
        {
            renderParameters = BuildRenderParameters(files);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"ERROR: failed to load docs-params: {ex.Message}");
            if (useGitHubAnnotations)
            {
                Console.WriteLine($"::error::docs-params: {EscapeAnnotation(ex.Message)}");
            }
            return 1;
        }

        var errorCount = 0;
        var warningCount = 0;
        var fileIssueCount = 0;
        var repoRoot = TryFindRepoRoot(Directory.GetCurrentDirectory());

        foreach (var file in files)
        {
            var fileIssues = CheckFile(file, renderParameters);
            if (fileIssues.Count == 0)
            {
                continue;
            }

            fileIssueCount++;

            foreach (var issue in fileIssues)
            {
                if (issue.Severity == IssueSeverity.Error)
                {
                    errorCount++;
                }
                else
                {
                    warningCount++;
                }

                var displayPath = repoRoot != null
                    ? Path.GetRelativePath(repoRoot, file)
                    : file;

                var severityLabel = issue.Severity == IssueSeverity.Error ? "error" : "warning";

                Console.WriteLine(
                    $"{displayPath}:{issue.Line}:{issue.Column}: {severityLabel}: [{issue.Kind}] {issue.Message}");

                if (useGitHubAnnotations)
                {
                    var command = issue.Severity == IssueSeverity.Error ? "error" : "warning";
                    Console.WriteLine(
                        $"::{command} file={displayPath},line={issue.Line},col={issue.Column}::" +
                        $"{issue.Kind}: {EscapeAnnotation(issue.Message)}");
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Checked {files.Count} markdown file(s). " +
                          $"{fileIssueCount} file(s) with issues, " +
                          $"{errorCount} error(s), {warningCount} warning(s).");

        if (errorCount > 0 || warningCount > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Tip: wrap inline Scriban-looking text with `{%{{{ ... }}}%}` " +
                              "or wrap whole code blocks with `{%{` ... `}%}` to escape Scriban parsing.");
        }

        return errorCount > 0 ? 1 : 0;
    }

    private static List<Issue> CheckFile(string file, IReadOnlyDictionary<string, string> renderParameters)
    {
        var issues = new List<Issue>();
        string content;
        try
        {
            content = File.ReadAllText(file);
        }
        catch (Exception ex)
        {
            issues.Add(new Issue("Read", 1, 1, ex.Message, IssueSeverity.Error));
            return issues;
        }

        var template = Template.Parse(content, file);

        foreach (var message in template.Messages)
        {
            var severity = message.Type switch
            {
                Scriban.Parsing.ParserMessageType.Error => IssueSeverity.Error,
                Scriban.Parsing.ParserMessageType.Warning => IssueSeverity.Warning,
                _ => (IssueSeverity?)null
            };

            if (severity is null)
            {
                continue;
            }

            var kind = severity == IssueSeverity.Error ? "ScribanParseError" : "ScribanParseWarning";

            issues.Add(new Issue(
                kind,
                message.Span.Start.Line + 1,
                message.Span.Start.Column + 1,
                message.Message,
                severity.Value));
        }

        if (template.HasErrors)
        {
            return issues;
        }

        try
        {
            var context = new TemplateContext
            {
                StrictVariables = true
            };

            var scriptObject = new ScriptObject();
            foreach (var entry in renderParameters)
            {
                scriptObject[entry.Key] = entry.Value;
            }

            context.PushGlobal(scriptObject);
            template.Render(context);
        }
        catch (ScriptRuntimeException ex)
        {
            issues.Add(new Issue(
                "ScribanRenderError",
                ex.Span.Start.Line + 1,
                ex.Span.Start.Column + 1,
                ex.OriginalMessage,
                IssueSeverity.Error));
        }
        catch (Exception ex)
        {
            issues.Add(new Issue("ScribanRenderError", 1, 1, ex.Message, IssueSeverity.Error));
        }

        return issues;
    }

    private static Dictionary<string, string> BuildRenderParameters(IEnumerable<string> files)
    {
        // Reproduces the keys the docs renderer places into its parameter
        // dictionary before rendering a documentation page.
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var name in BuiltInVariables)
        {
            parameters[name] = string.Empty;
        }

        foreach (var paramName in DiscoverParameterNames(files))
        {
            parameters[paramName] = string.Empty;
            parameters[paramName + "_Value"] = string.Empty;
        }

        return parameters;
    }

    private static HashSet<string> DiscoverParameterNames(IEnumerable<string> files)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        var visitedDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            var dir = Path.GetDirectoryName(file);
            while (!string.IsNullOrEmpty(dir) && visitedDirs.Add(dir))
            {
                var candidate = Path.Combine(dir, DocsParamsFileName);
                if (File.Exists(candidate))
                {
                    AddNamesFromDocsParamsFile(candidate, names);
                }

                var parent = Path.GetDirectoryName(dir);
                if (string.IsNullOrEmpty(parent) || parent == dir)
                {
                    break;
                }

                dir = parent;
            }
        }

        return names;
    }

    private static void AddNamesFromDocsParamsFile(string path, HashSet<string> sink)
    {
        // A malformed docs-params.json would silently shrink the injected
        // variable set and turn parameter file regressions into hard-to-debug
        // "variable not found" errors on otherwise-fine markdown. Surface the
        // failure directly so the contributor fixes the JSON instead.
        using var doc = JsonDocument.Parse(File.ReadAllText(path));

        if (!doc.RootElement.TryGetProperty("parameters", out var parameters) ||
            parameters.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException(
                $"{path}: expected a top-level `parameters` array.");
        }

        foreach (var parameter in parameters.EnumerateArray())
        {
            if (parameter.TryGetProperty("name", out var nameElement) &&
                nameElement.ValueKind == JsonValueKind.String)
            {
                var name = nameElement.GetString();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    sink.Add(name);
                }
            }
        }
    }

    private static string? TryFindRepoRoot(string startDir)
    {
        var current = new DirectoryInfo(startDir);
        while (current != null)
        {
            var gitPath = Path.Combine(current.FullName, ".git");
            if (Directory.Exists(gitPath) || File.Exists(gitPath))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return null;
    }

    private static string EscapeAnnotation(string text)
    {
        // GitHub workflow command escaping. `%` must be encoded first so the
        // sequences we introduce below are not double-escaped.
        return text
            .Replace("%", "%25")
            .Replace("\r", "%0D")
            .Replace("\n", "%0A")
            .Replace(":", "%3A")
            .Replace(",", "%2C");
    }

    private enum IssueSeverity
    {
        Warning,
        Error
    }

    private readonly record struct Issue(
        string Kind,
        int Line,
        int Column,
        string Message,
        IssueSeverity Severity);
}
