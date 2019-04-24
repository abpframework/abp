/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/IViewTree
* 创建者：天上有木月
* 创建时间：2019/4/3 10:39:05
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrganizationService
{
    public class ViewTree
    {
        public ViewTree()
        {
            Children = new List<ViewTree>();
        }

        public Guid Guid { get; set; }

        public string Title { get; set; }

        public List<ViewTree> Children { get; set; }

        public bool Expand { get; set; } = true;

        public Guid? ParentGuid { get; set; }

        public bool DisableCheckbox { get; set; } = false;
        public bool Checked { get; set; }

        public bool Selected { get; set; } = false;


    }


}
