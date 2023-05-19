using System;
using System.IO;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity.PlugIns;

public static class PlugInSourceListExtensions
{
    public static void AddFolder(
        [NotNull] this PlugInSourceList list,
        [NotNull] string folder,
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        Check.NotNull(list, nameof(list));

        list.Add(new FolderPlugInSource(folder, searchOption));
    }

    public static void AddTypes(
        [NotNull] this PlugInSourceList list,
        params Type[] moduleTypes)
    {
        Check.NotNull(list, nameof(list));

        list.Add(new TypePlugInSource(moduleTypes));
    }

    public static void AddFiles(
        [NotNull] this PlugInSourceList list,
        params string[] filePaths)
    {
        Check.NotNull(list, nameof(list));

        list.Add(new FilePlugInSource(filePaths));
    }
}
