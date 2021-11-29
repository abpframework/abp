using JetBrains.Annotations;

namespace Volo.Abp.Minify
{
    public interface IMinifier
    {
        string Minify(
            string source,
            [CanBeNull] string fileName = null,
            [CanBeNull] string originalFileName = null);
    }
}