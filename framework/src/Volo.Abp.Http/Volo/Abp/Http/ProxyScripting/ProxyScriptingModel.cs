using System.Collections.Generic;

namespace Volo.Abp.Http.ProxyScripting
{
    public class ProxyScriptingModel
    {
        public string GeneratorType { get; set; }

        public bool UseCache { get; set; }

        public bool Minify { get; set; }

        public string[] Modules { get; set; }

        public string[] Controllers { get; set; }

        public string[] Actions { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public ProxyScriptingModel(string generatorType, bool useCache = true, bool minify = false)
        {
            GeneratorType = generatorType;
            UseCache = useCache;
            Minify = minify;

            Properties = new Dictionary<string, string>();
        }

        public bool IsPartialRequest()
        {
            return !(Modules.IsNullOrEmpty() && Controllers.IsNullOrEmpty() && Actions.IsNullOrEmpty());
        }
    }
}