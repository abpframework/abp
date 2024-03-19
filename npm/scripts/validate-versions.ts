import { program } from "commander";
import fse from "fs-extra";
import * as path from "path";
import { log } from "./utils/log";

let excludedPackages = [];

(async () => {
  initCommander();
  await compare();
})();

function initCommander() {
  program.requiredOption(
    "-v, --compareVersion <version>",
    "version to compare"
  );
  program.requiredOption("-p, --path <path>", "NPM packages folder path");
  program.option(
    "-ep, --excludedPackages <excludedpackages>",
    "Packages that will not be checked. Can be passed with separeted comma (like @abp/utils,@abp/core)",
    ""
  );
  program.parse(process.argv);

  excludedPackages = program.opts().excludedPackages.split(",");
}

async function compare() {
  let { compareVersion, path: packagesPath } = program.opts();
  packagesPath = path.resolve(packagesPath);

  const packageFolders = await fse.readdir(packagesPath);

  for (let i = 0; i < packageFolders.length; i++) {
    const folder = packageFolders[i];
    const pkgJsonPath = `${packagesPath}/${folder}/package.json`;
    let pkgJson;

    try {
      pkgJson = await fse.readJSON(pkgJsonPath);
    } catch (error) {}

    if (
      !excludedPackages.includes(pkgJson.name) &&
      pkgJson.version !== compareVersion
    ) {
      throwError(pkgJsonPath, pkgJson.name, pkgJson.version);
    }

    const { dependencies } = pkgJson;
    if (dependencies) {
      await compareDependencies(dependencies, pkgJsonPath);
    }
  }
}

async function compareDependencies(
  dependencies: Record<string, string>,
  filePath: string
) {
  const { compareVersion } = program.opts();
  const entries = Object.entries(dependencies);

  for (let i = 0; i < entries.length; i++) {
    const entry = entries[i];
    const packageName = entry[0];
    const version = getCleanVersionName(entry[1]);
    const cleanCompareVersion = getCleanVersionName(compareVersion);

    if (
      !excludedPackages.includes(entry[0]) &&
      packageName.match(/@(abp|volo)/)?.length &&
      version !== cleanCompareVersion
    ) {
      throwError(filePath, entry[0], cleanCompareVersion);
    }
  }
}

function throwError(filePath: string, pkg: string, version?: string) {
  const { compareVersion } = program.opts();

  log.error(
    `${filePath}: ${pkg} version is not ${compareVersion}. it is ${version}`
  );
  process.exit(1);
}

function getCleanVersionName(version) {
  // Remove caret (^) or tilde (~) from the beginning of the version number
  return version.replace(/^[\^~]+/, "");
}
