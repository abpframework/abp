using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating.Razor
{
    public class DefaultCompiledViewProvider : ICompiledViewProvider, ITransientDependency
    {
        private static readonly ConcurrentDictionary<string, Assembly> CachedAssembles = new ConcurrentDictionary<string, Assembly>();

        private readonly CSharpCompiler _cSharpCompiler;
        private readonly IRazorProjectEngineFactory _razorProjectEngineFactory;
        private readonly ITemplateContentProvider _templateContentProvider;

        public DefaultCompiledViewProvider(
            IRazorProjectEngineFactory razorProjectEngineFactory,
            CSharpCompiler cSharpCompiler,
            ITemplateContentProvider templateContentProvider)
        {
            _razorProjectEngineFactory = razorProjectEngineFactory;
            _cSharpCompiler = cSharpCompiler;
            _templateContentProvider = templateContentProvider;
        }

        public virtual async Task<Assembly> GetAssemblyAsync(TemplateDefinition templateDefinition)
        {
            async Task<Assembly> CreateAssembly(string content)
            {
                using (var assemblyStream = await GetAssemblyStreamAsync(templateDefinition, content))
                {
                    return Assembly.Load(await assemblyStream.GetAllBytesAsync());
                }
            }

            var templateContent = await _templateContentProvider.GetContentOrNullAsync(templateDefinition);
            return CachedAssembles.GetOrAdd((templateDefinition.Name + templateContent).ToMd5(), await CreateAssembly(templateContent));
        }

        protected virtual async Task<Stream> GetAssemblyStreamAsync(TemplateDefinition templateDefinition, string templateContent)
        {
            var razorProjectEngine = await _razorProjectEngineFactory.CreateAsync(builder =>
            {
                builder.SetNamespace(AbpRazorTemplateConsts.DefaultNameSpace);
                builder.ConfigureClass((document, node) =>
                {
                    node.ClassName = AbpRazorTemplateConsts.DefaultClassName;
                });
            });

            var codeDocument = razorProjectEngine.Process(
                RazorSourceDocument.Create(templateContent, templateDefinition.Name), null,
                new List<RazorSourceDocument>(), new List<TagHelperDescriptor>());

            var cSharpDocument = codeDocument.GetCSharpDocument();

            return _cSharpCompiler.CreateAssembly(cSharpDocument.GeneratedCode, templateDefinition.Name);
        }
    }
}
