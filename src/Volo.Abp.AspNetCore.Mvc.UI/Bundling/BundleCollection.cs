using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.Bundling
{
    public class BundleCollection
    {
        private readonly Dictionary<string, List<string>> _bundles;

        public BundleCollection()
        {
            _bundles = new Dictionary<string, List<string>>();
        }

        public void Add(string bundleName, string[] files)
        {
            var list = _bundles.GetOrAdd(bundleName, () => new List<string>());
            list.AddRange(files);
        }

        public List<string> GetFiles(string bundleName)
        {
            var files = _bundles.GetOrDefault(bundleName);
            if (files == null)
            {
                throw new AbpException("Undefined bundle: " + bundleName);
            }

            return files;
        }
    }
}