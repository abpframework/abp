using System;
using Volo.Abp.Validation;
using Volo.CmsKit.RobotsTxts;

namespace Volo.CmsKit.Admin.RobotsTxts;

[Serializable]
public class UpdateRobotsTxtDto
{
    [DynamicStringLength(typeof(RobotsTxtConsts), nameof(RobotsTxtConsts.MaxContentLength))]
    public string Content { get; set; }
}
