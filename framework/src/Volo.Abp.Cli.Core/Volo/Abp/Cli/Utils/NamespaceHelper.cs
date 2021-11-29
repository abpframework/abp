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

            value = value.Trim();
            value = Regex.Replace(value, @"(((?<=\.)|^)((?=\d)|\.)|[^\w\.])|(\.$)", "_");

            return value;
        }
    }
}