using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.VirtualFileSystem.Embedded;

namespace Volo.Abp.VirtualFileSystem
{
    public static class VirtualFileSetListExtensions
    {
        public static void AddEmbedded<T>([NotNull] this VirtualFileSetList list, [CanBeNull] string baseNamespace = null, string baseFolderInProject = null)
        {
            Check.NotNull(list, nameof(list));

            list.Add(
                new EmbeddedFileSet(
                    typeof(T).Assembly,
                    baseNamespace,
                    baseFolderInProject
                )
            );
        }

        public static void ReplaceEmbeddedByPyhsical<T>([NotNull] this VirtualFileSetList list, [NotNull] string pyhsicalPath)
        {
            Check.NotNull(list, nameof(list));
            Check.NotNull(pyhsicalPath, nameof(pyhsicalPath));

            var assembly = typeof(T).Assembly;
            var embeddedFileSets = list.OfType<EmbeddedFileSet>().Where(fs => fs.Assembly == assembly).ToList();

            foreach (var embeddedFileSet in embeddedFileSets)
            {
                list.Remove(embeddedFileSet);

                if (!embeddedFileSet.BaseFolderInProject.IsNullOrEmpty())
                {
                    pyhsicalPath = Path.Combine(pyhsicalPath, embeddedFileSet.BaseFolderInProject);
                }

                list.PhysicalPaths.Add(pyhsicalPath);
            }
        }
    }
}