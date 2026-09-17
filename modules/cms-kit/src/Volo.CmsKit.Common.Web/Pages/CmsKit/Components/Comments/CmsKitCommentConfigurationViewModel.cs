using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.Comments;

public class CmsKitCommentConfigurationViewModel
{
  [Required]
  public string EntityType { get; set; }

  [Required]
  public string EntityId { get; set; }

  public bool IsReadOnly { get; set; } = false;
}