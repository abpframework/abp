namespace Volo.Abp.Minify
{
    public interface IMinifier
    {
        string Minify(string source, string fileName = null);
    }
}