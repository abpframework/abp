import { program } from "commander";
import childProcess from "child_process";

/**
 * This script is used to update the version of the LeptonX (angular |mvc | blazor) npm packages.
 * Depending to **change-package-version.ts** file. (Should we move this to a single script?)
 *
 * I'm not sure about depending "commander" package. We might need to get options from process.env directly ?
 *
 * Example
 * -Set env
 *   $env:targetVersion = "1.0.0"
 * -Read from nodejs
 *   process.env.targetVersion
 * -Use as argument in commands
 *   const command = `yarn change-package-version -n ${packageName} -v ${process.env.targetVersion}`;
 */

//All lepton-x-lite packages for open source (angular | mvc | blazor) UI frameworks
const LEPTON_X_PACKAGE_NAMES = [
  "@abp/ng.theme.lepton-x",
  "@abp/aspnetcore.mvc.ui.theme.leptonxlite",
  "@abp/aspnetcore.components.server.leptonxlitetheme",
];

function validateVersion(targetVersion) {
  if (!targetVersion) {
    console.log("\x1b[31m", "Error: lepton-x targetVersion is not defined");
    process.exit(1);
  }
}

function initCommander() {
  program.option(
    "-v, --targetVersion <targetVersion>",
    "Version number of the package"
  );

  program.parse(process.argv);
}

(() => {
  initCommander();

  const { targetVersion } = program.opts();

  validateVersion(targetVersion);

  LEPTON_X_PACKAGE_NAMES.forEach((packageName) => {
    const command = `yarn change-package-version -n ${packageName} -v ${targetVersion}`;
    const result = childProcess.execSync(command).toString();

    console.log(result);
  });
})();
