/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/ViewTreeExtensions
* 创建者：天上有木月
* 创建时间：2019/4/3 10:52:24
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
    public static class ViewTreeExtensions
    {

        public static List<ViewTree> ComboboxTreeJson(this List<ViewTree> data, Guid? parentId = null)
        {
            List<ViewTree> listTreeNodes = new List<ViewTree>();

            ComboboxTreeJson(data, listTreeNodes, parentId);
            return listTreeNodes;
        }


        private static void ComboboxTreeJson(List<ViewTree> listModels, List<ViewTree> listTreeNodes, Guid? pid)
        {
            foreach (ViewTree item in listModels)
            {
                if (!item.ParentGuid.Equals(pid)) continue;


                bool hasChildren = listModels.Any(u => u.ParentGuid == item.Guid);


                listTreeNodes.Add(item);


                ComboboxTreeJson(listModels, item.Children, item.Guid);
            }
        }

    }
}
