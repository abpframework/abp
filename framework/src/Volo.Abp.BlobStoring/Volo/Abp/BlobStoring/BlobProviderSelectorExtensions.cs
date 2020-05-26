using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public static class BlobProviderSelectorExtensions
    {
        public static IBlobProvider Get<TContainer>(
            [NotNull] IBlobProviderSelector selector)
        {
            Check.NotNull(selector, nameof(selector));

            return selector.Get(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }
    }
}