namespace Volo.Abp.AspNetCore.Mvc
{
    public interface IUrlActionNameNormalizer
    {
        void Normalize(UrlActionNameNormalizerContext context);
    }
}