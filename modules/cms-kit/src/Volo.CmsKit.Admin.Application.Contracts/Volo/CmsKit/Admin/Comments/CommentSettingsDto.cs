using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class SettingsDto
{
    public bool RequireApprovement { get; set; }
}
