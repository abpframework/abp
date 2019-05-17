using System.IO;
using Volo.Abp.DependencyInjection;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Volo.Docs.Yaml
{
    public class YamlMetadataParser : IYamlMetadataParser, ITransientDependency
    {
        private readonly IDeserializer _yamlDeserializer;

        public YamlMetadataParser()
        {
            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }

        public YamlMetadata ParseOrNull(string markdown)
        {
            try
            {
                using (var input = new StringReader(markdown))
                {
                    var parser = new Parser(input);
                    parser.Expect<StreamStart>();
                    parser.Expect<DocumentStart>();
                    var metadata = _yamlDeserializer.Deserialize<YamlMetadata>(parser);
                    parser.Expect<DocumentEnd>();
                    return metadata;
                }
            }
            catch (YamlException)
            {
                return null;
            }
        }
    }
}