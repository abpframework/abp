using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating.Razor
{
    public class AbpRazorTemplateCSharpCompiler : ISingletonDependency
    {
        protected AbpRazorTemplateCSharpCompilerOptions Options { get; }

        public AbpRazorTemplateCSharpCompiler(IOptions<AbpRazorTemplateCSharpCompilerOptions> options)
        {
            Options = options.Value;
        }

        private static IEnumerable<PortableExecutableReference> DefaultReferences => new List<Assembly>()
            {
                Assembly.Load("netstandard"),
                Assembly.Load("System.Private.CoreLib"),
                Assembly.Load("System.Runtime"),
                Assembly.Load("System.Collections"),
                Assembly.Load("System.ComponentModel"),
                Assembly.Load("System.Linq"),
                Assembly.Load("System.Linq.Expressions"),
                Assembly.Load("Microsoft.Extensions.DependencyInjection"),
                Assembly.Load("Microsoft.Extensions.DependencyInjection.Abstractions"),
                Assembly.Load("Microsoft.Extensions.Localization"),
                Assembly.Load("Microsoft.Extensions.Localization.Abstractions"),

                typeof(AbpRazorTemplateCSharpCompiler).Assembly
            }
            .Select(x => MetadataReference.CreateFromFile(x.Location))
            .ToImmutableList();

        public virtual Stream CreateAssembly(string code, string assemblyName, List<MetadataReference> references = null, CSharpCompilationOptions options = null)
        {
            var defaultReferences = DefaultReferences.Concat(Options.References);
            try
            {
                var compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: new[] { CreateSyntaxTree(code) },
                    references: references != null ? defaultReferences.Concat(references) : defaultReferences,
                    options ?? GetCompilationOptions());

                using (var memoryStream = new MemoryStream())
                {
                    var result = compilation.Emit(memoryStream);

                    if (!result.Success)
                    {
                        var error = new StringBuilder();
                        error.AppendLine("Build failed");
                        foreach (var diagnostic in result.Diagnostics)
                        {
                            error.AppendLine(diagnostic.ToString());
                        }

                        throw new Exception(error.ToString());
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var assemblyStream = new MemoryStream();
                    memoryStream.CopyTo(assemblyStream);

                    return assemblyStream;
                }
            }
            catch (Exception e)
            {
                var error = new StringBuilder();
                error.AppendLine("CreateAssembly failed");
                error.AppendLine(e.Message);
                throw new Exception(error.ToString());
            }
        }

        protected virtual SyntaxTree CreateSyntaxTree(string code)
        {
            return CSharpSyntaxTree.ParseText(code);
        }

        protected virtual CSharpCompilationOptions GetCompilationOptions()
        {
            var csharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // Disable 1702 until roslyn turns this off by default
            csharpCompilationOptions = csharpCompilationOptions.WithSpecificDiagnosticOptions(
                new Dictionary<string, ReportDiagnostic>
                {
                    {"CS1701", ReportDiagnostic.Suppress}, // Binding redirects
                    {"CS1702", ReportDiagnostic.Suppress},
                    {"CS1705", ReportDiagnostic.Suppress}
                });

            return csharpCompilationOptions.WithOptimizationLevel(OptimizationLevel.Release);
        }
    }
}
