using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.Menus;

[Serializable]
public class MenuUpdateInput : ExtensibleObject
{
    public string Name { get; set; }
}
