namespace Volo.Docs.Markdown
{
    public interface IMarkdownConverter
    {
        string ConvertToHtml(string markdown);
    }
}