using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Volo.Abp.Cli.Utils
{
    public static class NamespaceHelper
    {
        public static string NormalizeNamespace([CanBeNull] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = Regex.Replace(value, @"(^\s+|\s+$)", "");
            value = Regex.Replace(value, @"(((?<=\.)|^)((?=\d)|\.)|[^\w\.])|(\.$)", "_");

            return value;
        }
    }
}