import execa from "execa";
import fse from "fs-extra";
import fs from "fs";
import program from "commander";

const defaultTemplates = ['app', 'app-nolayers', 'module'];
const defaultTemplatePath = '../../../templates';
const packageMap = {
  account: 'ng.account',
  'account-core': 'ng.account.core',
  components: 'ng.components',
  core: 'ng.core',
  'feature-management': 'ng.feature-management',
  identity: 'ng.identity',
  'permission-management': 'ng.permission-management',
  'setting-management': 'ng.setting-management',
  'tenant-management': 'ng.tenant-management',
  'theme-basic': 'ng.theme.basic',
  'theme-shared': 'ng.theme.shared',
  schematics: 'ng.schematics',
  oauth: 'ng.oauth',
};
program.option('-t, --templates  <templates>', 'template dirs', false);
program.option('-p, --template-path <templatePath>', 'root template path', false);
program.option(
  '-e, --use-existing-build <useExistingBuild>',
  "don't build packages if dist folder exists",
  false,
);
program.option('-i, --noInstall', 'skip package installation', false);
program.option('-a, --absolute-dir <absoluteDir>', 'Absolute angular directory', false);
program.parse(process.argv);
const templates = program.templates ? program.templates.split(',') : defaultTemplates;
const templateRootPath = program.templatePath ? program.templatePath : defaultTemplatePath;
(async () => {
  if (!program.useExistingBuild) {
    await execa('yarn', ['build:all'], {
      stdout: 'inherit',
      cwd: '../',
    });
  }

  if (!program.noInstall) {
    await installPackages();
  }

  await removeAbpPackages();

  await copyBuildedPackagesFromDistFolder();
})();

async function runEachTemplate(
  handler: (template: string, templatePath?: string) => void | Promise<any>,
) {
  if (program.absoluteDir) {
    const result = handler('', program.absoluteDir);
    result instanceof Promise ? await result : result;
  } else {
    for (var template of templates) {
      const templatePath = `${templateRootPath}/${template}/angular`;
      const result = handler(template, templatePath);
      result instanceof Promise ? await result : result;
    }
  }

}

async function installPackages() {
  await runEachTemplate(async (template, templatePath) => {
    if (fse.existsSync(`${templatePath}/yarn.lock`)) {
      fse.removeSync(`${templatePath}/yarn.lock`);
    }
    await execa('yarn', ['install', '--ignore-scripts'], {
      stdout: 'inherit',
      cwd: templatePath,
    });
  });
}

async function removeAbpPackages() {
  await runEachTemplate(async (template, templatePath) => {
    Object.values(packageMap).forEach(value => {
      const path = `${templatePath}/node_modules/@abp/${value}`;
      if (fs.existsSync(path)) {
        fse.removeSync(path);
      }
    });
    if (fs.existsSync(`${templatePath}/.angular`)) {
      fse.removeSync(`${templatePath}/.angular`);
    }
  });
}
function createFolderIfNotExists(destination: string) {
  destination.split('/').reduce((acc, dir) => {
    if (!fs.existsSync(acc)) {
      fs.mkdirSync(acc);
    }
    return `${acc}/${dir}`;
  });
}
async function copyBuildedPackagesFromDistFolder() {
  await runEachTemplate(async (template, templatePath) => {
    Object.entries(packageMap).forEach(([key, value]) => {
      createFolderIfNotExists(`${templatePath}/node_modules/@abp/${value}`);
      fse.copySync(`../dist/packages/${key}/`, `${templatePath}/node_modules/@abp/${value}`);
    });
  });
}
