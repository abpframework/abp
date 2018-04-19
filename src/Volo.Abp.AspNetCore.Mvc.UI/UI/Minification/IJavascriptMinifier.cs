namespace Volo.Abp.AspNetCore.Mvc.UI.Minification
{
    public interface IJavascriptMinifier
    {
        string Minify(string source, string fileName = null);
    }
}