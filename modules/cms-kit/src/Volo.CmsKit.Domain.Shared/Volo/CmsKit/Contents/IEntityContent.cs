namespace Volo.CmsKit.Domain.Shared.Volo.CmsKit.Contents
{
    public interface IEntityContent : IContent
    {
        string EntityType { get; set; }
        string EntityId { get; set; }
    }
}
