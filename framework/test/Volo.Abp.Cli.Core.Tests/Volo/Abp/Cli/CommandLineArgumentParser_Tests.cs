using Shouldly;
using System.Linq;
using Volo.Abp.Cli.Args;
using Xunit;

namespace Volo.Abp.Cli
{
    public class CommandLineArgumentParser_Tests : CliTestBase
    {
        private readonly ICommandLineArgumentParser _commandLineArgumentParser;

        public CommandLineArgumentParser_Tests()
        {
            _commandLineArgumentParser = GetRequiredService<ICommandLineArgumentParser>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new string[0] })]
        public void Should_Parse_Empty_Arguments(string[] args)
        {
            var commandLineArgs = _commandLineArgumentParser.Parse(args);
            commandLineArgs.Command.ShouldBeNull();
            commandLineArgs.Target.ShouldBeNull();
            commandLineArgs.Options.Any().ShouldBeFalse();
        }

        [Fact]
        public void Should_Work_With_Only_Command()
        {
            var commandLineArgs = _commandLineArgumentParser.Parse(new[] { "mycommand" });
            commandLineArgs.Command.ShouldBe("mycommand");
            commandLineArgs.Target.ShouldBeNull();
            commandLineArgs.Options.Any().ShouldBeFalse();
        }

        [Fact]
        public void Should_Work_With_Command_And_Target()
        {
            var commandLineArgs = _commandLineArgumentParser.Parse(new[] { "mycommand", "mytarget" });
            commandLineArgs.Command.ShouldBe("mycommand");
            commandLineArgs.Target.ShouldBe("mytarget");
            commandLineArgs.Options.Any().ShouldBeFalse();
        }

        [Fact]
        public void Should_Work_With_Command_And_Target_And_Options()
        {
            var commandLineArgs = _commandLineArgumentParser.Parse(new[]
            {
                "mycommand", "mytarget", "-a", "value1", "-b", "--optionC", "value2"
            });

            commandLineArgs.Command.ShouldBe("mycommand");
            commandLineArgs.Target.ShouldBe("mytarget");
            commandLineArgs.Options.Any().ShouldBeTrue();

            commandLineArgs.Options.ShouldContainKey("a");
            commandLineArgs.Options["a"].ShouldBe("value1");

            commandLineArgs.Options.ShouldContainKey("b");
            commandLineArgs.Options["b"].ShouldBeNull();

            commandLineArgs.Options.ShouldContainKey("optionC");
            commandLineArgs.Options["optionC"].ShouldBe("value2");
        }
    }
}
