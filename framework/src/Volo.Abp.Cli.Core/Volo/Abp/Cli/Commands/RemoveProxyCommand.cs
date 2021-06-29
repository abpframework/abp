namespace Volo.Abp.Cli.Commands
{
    public class RemoveProxyCommand : ProxyCommandBase
    {
        public const string Name = "remove-proxy";

        protected override string CommandName => Name;

        protected override string SchematicsCommandName => "proxy-remove";

        public RemoveProxyCommand(CliService cliService)
            : base(cliService)
        {
        }

        public override string GetShortDescription()
        {
            return "Remove Angular service proxies and DTOs to consume HTTP APIs.";
        }
    }
}
