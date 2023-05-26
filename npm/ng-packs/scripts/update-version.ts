import fse from 'fs-extra';
import execa from 'execa';
import program from 'commander';

program
  .option(
    '-v, --nextVersion <version>',
    'next semantic version. Available versions: ["major", "minor", "patch", "premajor", "preminor", "prepatch", "prerelease", "or type a custom version"]',
  );
  program.parse(process.argv);


(async () => {
    await updateVersion(program.nextVersion);
})();


async function updateVersion(version: string) {
    if(!version){
        console.error('Please provide a version with --nextVersion attribute');
        return;
    }
    await fse.rename('../lerna.version.json', '../lerna.json');
  
    await execa(
      'yarn',
      ['lerna', 'version', version, '--yes', '--no-commit-hooks', '--skip-git', '--force-publish'],
      { stdout: 'inherit', cwd: '../' },
    );
  
    await fse.rename('../lerna.json', '../lerna.version.json');
  
   await execa('yarn', ['replace-with-tilde']);
  }