using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public interface ILocalizedTemplateContentReader
    {
        public string GetContentOrNull([CanBeNull] string culture);
    }
}