import execa from 'execa';
import fse from 'fs-extra';
import glob from 'glob';

async function replace(filePath: string) {
  const pkg = await fse.readJson(filePath);

  const { dependencies } = pkg;

  if (!dependencies) return;

  Object.keys(dependencies).forEach(key => {
    if (key.includes('@abp/')) {
      dependencies[key] = dependencies[key].replace('^', '~');
    }
  });

  await fse.writeJson(filePath, { ...pkg, dependencies }, { spaces: 2 });
}

glob('../packages/**/package.json', {}, (er, files) => {
  files.forEach(path => {
    if (path.includes('node_modules')) {
      return;
    }

    replace(path);
  });
});
