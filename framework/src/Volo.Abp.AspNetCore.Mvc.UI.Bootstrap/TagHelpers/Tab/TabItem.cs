namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab;

public class TabItem
{
    public TabItem(string header, string content, bool active, string id, string parentId, bool isDropdown)
    {
        Header = header;
        Content = content;
        Active = active;
        Id = id;
        ParentId = parentId;
        IsDropdown = isDropdown;
    }

    public string Header { get; set; }

    public string Content { get; set; }

    public bool Active { get; set; }

    public bool IsDropdown { get; set; }

    public string Id { get; set; }

    public string ParentId { get; set; }
}
