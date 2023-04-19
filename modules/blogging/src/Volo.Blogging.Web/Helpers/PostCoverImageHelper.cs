using System;
using System.Linq;

namespace Volo.Blogging.Helpers;

public static class PostCoverImageHelper
{
    private static readonly string[] PostColors =
    {
        "linear-gradient(135deg, #4b2d21 0%, #7d4b37 100%)",
        "linear-gradient(135deg, #41003d 0%, #a10097 100%)",
        "linear-gradient(135deg, #163e4b 0%, #297791 100%)",
        "linear-gradient(135deg, #660040 0%, #aa006b 100%)",
        "linear-gradient(135deg, #240d88 0%, #571fff 100%)",
        "linear-gradient(135deg, #0d6a88 0%, #16b0e1 100%)",
        "linear-gradient(135deg, #329063 0%, #4be099 100%)",
        "linear-gradient(135deg, #904432 0%, #f07153 100%)",
        "linear-gradient(135deg, #903282 0%, #e04cca 100%)",
        "linear-gradient(135deg, #901943 0%, #f02a70 100%)",
        "linear-gradient(135deg, #2e634a 0%, #4da57b 100%)",
        "linear-gradient(135deg, #9c003f 0%, #e41d6f 100%)",
        "linear-gradient(135deg, #806419 0%, #d5a72a 100%)",
        "linear-gradient(135deg, #545975 0%, #8890bd 100%)",
        "linear-gradient(135deg, #67172a 0%, #ba2549 100%)"
    };

    public static string GetRandomColor(string postTitle)
    {
        long total = postTitle.Truncate(32).ToCharArray().Sum(c => c);
        return PostColors[total % PostColors.Length];
    }

    public static string GetTitleCapitals(string postTitle)
    {
        if (postTitle.Length < 2)
        {
            return postTitle.ToUpperInvariant();
        }

        if (postTitle.Contains(" "))
        {
            var splitted = postTitle.Split(" ");
            return (splitted[0][0].ToString() + splitted[1][0].ToString()).ToUpperInvariant();
        }

        return postTitle.ToUpperInvariant().Left(2);
    }
}