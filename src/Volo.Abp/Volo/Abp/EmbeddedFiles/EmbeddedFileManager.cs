using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EmbeddedFiles
{
    public class EmbeddedFileManager : IEmbeddedFileManager, ISingletonDependency
    {
        private readonly EmbeddedFileOptions _options;
        private readonly Lazy<Dictionary<string, EmbeddedFileInfo>> _resources;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EmbeddedFileManager(IOptions<EmbeddedFileOptions> options)
        {
            _options = options.Value;
            _resources = new Lazy<Dictionary<string, EmbeddedFileInfo>>(
                CreateResourcesDictionary,
                true
            );
        }

        /// <inheritdoc/>
        public EmbeddedFileInfo FindFile(string fullPath)
        {
            return _resources.Value.GetOrDefault(EmbeddedFilePathHelper.NormalizePath(fullPath));
        }

        private Dictionary<string, EmbeddedFileInfo> CreateResourcesDictionary()
        {
            var resources = new Dictionary<string, EmbeddedFileInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var source in _options.Sources)
            {
                source.AddResources(resources);
            }

            return resources;
        }
    }
}