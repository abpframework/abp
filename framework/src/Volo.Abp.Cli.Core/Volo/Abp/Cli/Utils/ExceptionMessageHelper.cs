namespace Volo.Abp.Cli.Utils;

public static class ExceptionMessageHelper
{
    public static string GetInvalidOptionExceptionMessage(string optionName) => $"The option you provided for {optionName} is invalid!";
} 