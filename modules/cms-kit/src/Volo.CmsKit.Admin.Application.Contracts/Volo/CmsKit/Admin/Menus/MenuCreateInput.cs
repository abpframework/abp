using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.Menus;

[Serializable]
public class MenuCreateInput : ExtensibleObject
{
    public string Name { get; set; }
}
