import { Argument, Command, Option } from 'commander';
import execa from 'execa';

(async () => {
  const settings = {
    "core": {
      m: "abp",
      a: "abp"
    }
  }
  const program = new Command();
  program.addArgument(new Argument('target-name', 'Target name').choices(Object.keys(settings)).argRequired())

  program.option('-u, --url <char>', 'Backend service url', "https://localhost:44305");
  program.parse(process.argv);
  const options = program.opts()
  const args = program.args
  try {

    const targetName = args[0];
    const parameters = settings[targetName]
    await execa(
      'abp',
      [
        "generate-proxy",
        "-t",
        "ng",
        "-m",
        parameters.m,
        "-a",
        parameters.a,
        "-u", options.url,
        "--target", "core"
      ],
      { stdout: 'inherit', cwd: '../' },
    );



  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
