import execa from 'execa';
import fse from 'fs-extra';

(async () => {
  rename(true);

  try {
    await execa('yarn', ['ngcc'], {
      stdout: 'inherit',
      cwd: '../',
    });
  } catch (error) {
    rename(false);
    process.exit(1);
  }

  rename(false);
})();

async function rename(prodToMain: boolean) {
  await fse.rename('../tsconfig.json', `../tsconfig.${prodToMain ? 'temp' : 'prod'}.json`);
  await fse.rename(`../tsconfig.${prodToMain ? 'prod' : 'temp'}.json`, '../tsconfig.json');
}
