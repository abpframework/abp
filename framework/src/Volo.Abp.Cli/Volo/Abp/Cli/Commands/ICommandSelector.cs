namespace Volo.Abp.Cli
{
    public interface ICommandSelector
    {
        IConsoleCommand Select(CommandLineArgs commandLineArgs);
    }
}