namespace Volo.Abp.MultiLingualObjects.TestObjects;

public class MultiLingualBookTranslation : IObjectTranslation
{
    public string? Name { get; set; }

    public required string Language { get; set; }
}
