using System.Text;
using CommonMark;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Formatting
{
    public class MarkdownDocumentFormatting : IDocumentFormatting, ITransientDependency
    {
        public const string Type = "md";

        public string Format(string content)
        {
            byte[] bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            return CommonMarkConverter.Convert(utf8Content);
        }
    }
}
