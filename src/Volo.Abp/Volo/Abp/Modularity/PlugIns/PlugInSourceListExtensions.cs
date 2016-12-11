using System;
using System.IO;

namespace Volo.Abp.Modularity.PlugIns
{
    public static class PlugInSourceListExtensions
    {
        public static void AddFolder(this PlugInSourceList list, string folder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            list.Add(new FolderPlugInSource(folder, searchOption));
        }

        public static void AddTypes(this PlugInSourceList list, params Type[] moduleTypes)
        {
            list.Add(new PlugInTypeListSource(moduleTypes));
        }
    }
}