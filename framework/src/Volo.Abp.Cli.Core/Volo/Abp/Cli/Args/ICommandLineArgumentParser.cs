namespace Volo.Abp.Cli.Args
{
    public interface ICommandLineArgumentParser
    {
        CommandLineArgs Parse(string[] args);
        
        CommandLineArgs Parse(string lineText);
    }
}