using System.IO;
using Microsoft.Extensions.Primitives;

namespace Volo.Abp.AspNetCore.VirtualFileSystem;

/* Inspired from the Microsoft.Extensions.FileProviders.Physical package. */
internal static class PathUtils
{
    private static readonly char[] PathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

    public static bool PathNavigatesAboveRoot(string path)
    {
        var tokenizer = new StringTokenizer(path, PathSeparators);
        var depth = 0;

        foreach (var segment in tokenizer)
        {
            if (segment.Equals(".") || segment.Equals(""))
            {
                continue;
            }

            if (segment.Equals(".."))
            {
                depth--;

                if (depth == -1)
                {
                    return true;
                }
            }
            else
            {
                depth++;
            }
        }

        return false;
    }
}
