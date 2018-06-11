namespace Volo.Abp.AspNetCore.Mvc.UI.Minification
{
    public interface IMinifier
    {
        string Minify(string source, string fileName = null);
    }
}