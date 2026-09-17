using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.Docs.Language;
using Volo.Docs.Projects;

namespace Volo.Docs.Areas.Models.DocumentNavigation;

public class GetNavigationNodeWithLinkModel
{
    public Guid ProjectId { get; set; }

    [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxVersionNameLength))]
    public string Version { get; set; }

    [Required]
    [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxLanguageCodeLength))]
    public string LanguageCode { get; set; }
    
    [Required]
    public string ProjectName { get; set; }
    
    [Required]
    public string ProjectFormat { get; set; }
    
    [Required]
    public string RouteVersion { get; set; }
}