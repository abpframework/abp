namespace Volo.Abp.Cli.Commands
{
    public class GenerateProxyCommand : ProxyCommandBase
    {
        public const string Name = "generate-proxy";

        protected override string CommandName => Name;

        protected override string SchematicsCommandName => "proxy-add";

        public GenerateProxyCommand(CliService cliService)
            : base(cliService)
        {
        }

        public override string GetShortDescription()
        {
            return "Generates Angular service proxies and DTOs to consume HTTP APIs.";
        }
    }
}
