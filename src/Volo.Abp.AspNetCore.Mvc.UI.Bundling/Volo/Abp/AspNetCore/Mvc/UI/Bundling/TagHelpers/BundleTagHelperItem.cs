using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class BundleTagHelperItem
    {
        public string File { get; }

        public Type Type { get; }

        public BundleTagHelperItem(string file)
        {
            File = file;
        }

        public BundleTagHelperItem(Type type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return File ?? Type.FullName ?? "?";
        }

        public void AddToConfiguration(BundleConfiguration configuration)
        {
            if (File != null)
            {
                configuration.AddFiles(File);
            }
            else if (Type != null)
            {
                configuration.AddContributors(Type);
            }
        }
    }
}