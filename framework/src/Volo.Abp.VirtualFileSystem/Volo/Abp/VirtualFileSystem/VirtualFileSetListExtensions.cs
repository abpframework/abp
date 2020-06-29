using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem.Embedded;

namespace Volo.Abp.VirtualFileSystem
{
    public static class VirtualFileSetListExtensions
    {
        public static void AddEmbedded<T>(
            [NotNull] this VirtualFileSetList list,
            [CanBeNull] string baseNamespace = null,
            [CanBeNull] string baseFolder = null)
        {
            Check.NotNull(list, nameof(list));

            var assembly = typeof(T).Assembly;
            var fileProvider = CreateFileProvider(
                assembly,
                baseNamespace,
                baseFolder
            );

            list.Add(new EmbeddedVirtualFileSetInfo(fileProvider, assembly, baseFolder));
        }

        private static IFileProvider CreateFileProvider(
            [NotNull] Assembly assembly,
            [CanBeNull] string baseNamespace = null,
            [CanBeNull] string baseFolder = null)
        {
            Check.NotNull(assembly, nameof(assembly));

            var info = assembly.GetManifestResourceInfo("Microsoft.Extensions.FileProviders.Embedded.Manifest.xml");

            if (info == null)
            {
                return new EmbeddedFileSet(assembly, baseNamespace);
            }

            if (baseFolder == null)
            {
                return new ManifestEmbeddedFileProvider(assembly);
            }

            return new ManifestEmbeddedFileProvider(assembly, baseFolder);
        }

        public static void ReplaceEmbeddedByPhysical<T>(
            [NotNull] this VirtualFileSetList fileSets,
            [NotNull] string pyhsicalPath)
        {
            Check.NotNull(fileSets, nameof(fileSets));
            Check.NotNullOrWhiteSpace(pyhsicalPath, nameof(pyhsicalPath));

            var assembly = typeof(T).Assembly;

            for (var i = 0; i < fileSets.Count; i++)
            {
                if (fileSets[i] is EmbeddedVirtualFileSetInfo embeddedVirtualFileSet &&
                    embeddedVirtualFileSet.Assembly == assembly)
                {
                    var thisPath = pyhsicalPath;

                    if (!embeddedVirtualFileSet.BaseFolder.IsNullOrEmpty())
                    {
                        thisPath = Path.Combine(thisPath, embeddedVirtualFileSet.BaseFolder);
                    }

                    fileSets[i] = new VirtualFileSetInfo(new PhysicalFileProvider(thisPath));
                }
            }
        }
    }
}