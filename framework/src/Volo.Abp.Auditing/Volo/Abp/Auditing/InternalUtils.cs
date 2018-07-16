using System;

namespace Volo.Abp.Auditing
{
    internal static class InternalUtils
    {
        internal static string AddCounter(string str)
        {
            if (str.Contains("__"))
            {
                var splitted = str.Split("__");
                if (splitted.Length == 2)
                {
                    if (int.TryParse(splitted[1], out var num))
                    {
                        return splitted[0] + "__" + (++num);
                    }
                }
            }

            return str + "__2";
        }
    }
}
