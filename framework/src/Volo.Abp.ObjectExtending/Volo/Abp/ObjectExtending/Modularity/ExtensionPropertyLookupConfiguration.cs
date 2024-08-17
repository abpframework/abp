namespace Volo.Abp.ObjectExtending.Modularity;

public class ExtensionPropertyLookupConfiguration
{
    public string Url { get; set; } = default!;

    /// <summary>
    /// Default value: "items".
    /// </summary>
    public string ResultListPropertyName { get; set; } = "items";

    /// <summary>
    /// Default value: "text".
    /// </summary>
    public string DisplayPropertyName { get; set; } = "text";

    /// <summary>
    /// Default value: "id".
    /// </summary>
    public string ValuePropertyName { get; set; } = "id";

    /// <summary>
    /// Default value: "filter".
    /// </summary>
    public string FilterParamName { get; set; } = "filter";
}
