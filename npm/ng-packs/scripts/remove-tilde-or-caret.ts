import fse from 'fs-extra';
import glob from 'glob';

async function replace(filePath: string) {
  const pkg = await fse.readJson(filePath);

  const { dependencies } = pkg;

  if (!dependencies) return;

  const packageNameRegex = /^@(abp|volo)/;
  const markRegex = /(~|^)/;
  Object.keys(dependencies).forEach(key => {
    if (packageNameRegex.test(key)) {
      dependencies[key] = dependencies[key].replace(markRegex, '');
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
