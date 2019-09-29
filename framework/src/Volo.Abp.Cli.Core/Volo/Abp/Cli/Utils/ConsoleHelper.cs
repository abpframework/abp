using System;
using System.Text;

namespace Volo.Abp.Cli.Utils
{
    public static class ConsoleHelper
    {
        public static string ReadSecret()
        {
            var sb = new StringBuilder();

            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                sb.Append(keyInfo.KeyChar);
            }

            return sb.ToString();
        }
    }
}