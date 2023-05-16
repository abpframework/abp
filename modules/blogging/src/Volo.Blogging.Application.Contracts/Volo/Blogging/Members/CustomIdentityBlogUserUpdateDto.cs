using System.ComponentModel.DataAnnotations;
using Volo.Blogging.Users;

namespace Volo.Blogging.Members;

public class CustomIdentityBlogUserUpdateDto
{
    [StringLength(UserConsts.MaxNameLength)]
    public string Name { get; set; }

    [StringLength(UserConsts.MaxSurnameLength)]
    public string Surname { get; set; }

    [RegularExpression(@"^((?!\s).)*$", ErrorMessage= "PersonalSiteUrlValidationMessage")]
    [StringLength(UserConsts.MaxWebSiteLength)]
    public string WebSite { get; set; }

    [RegularExpression(@"^((?!\s).)*$", ErrorMessage= "TwitterUserNameValidationMessage")]
    [StringLength(UserConsts.MaxTwitterLength)]
    public string Twitter { get; set; }

    [RegularExpression(@"^((?!\s).)*$", ErrorMessage= "GitHubUserNameValidationMessage")]
    [StringLength(UserConsts.MaxGithubLength)]
    public string Github { get; set; }

    [RegularExpression(@"^((?!\s).)*$", ErrorMessage= "LinkedinUrlValidationMessage")]
    [StringLength(UserConsts.MaxLinkedinLength)]
    public string Linkedin { get; set; }

    [StringLength(UserConsts.MaxCompanyLength)]
    public string Company { get; set; }

    [StringLength(UserConsts.MaxJobTitleLength)]
    public string JobTitle { get; set; }

    [StringLength(UserConsts.MaxBiographyLength)]
    public string Biography { get; set; }
}