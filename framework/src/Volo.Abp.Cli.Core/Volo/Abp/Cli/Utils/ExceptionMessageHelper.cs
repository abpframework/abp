using System;
using System.Text;

namespace Volo.Abp.Cli.Utils;

public static class ExceptionMessageHelper
{
    public static string GetInvalidArgExceptionMessage(string args)
    {
        Check.NotNullOrEmpty(args, nameof(args), minLength: 2);

        var exceptionMessageBuilder = new StringBuilder();
        exceptionMessageBuilder.Append("The option you provided for ");
        if (args.Contains(" "))
        {
            foreach (var arg in args.Split(' '))
            {
                exceptionMessageBuilder.Append(arg.ToPascalCase());
            }
        }
        else
        {
            exceptionMessageBuilder.Append(args.ToPascalCase());
        }

        exceptionMessageBuilder.Append(" is invalid!");

        return exceptionMessageBuilder.ToString();
    }
} 