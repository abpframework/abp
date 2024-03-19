import glob from "glob";
import fse from "fs-extra";
import { program } from "commander";

export const semverRegex =
  /\d+\.\d+\.\d+(?:-[a-zA-Z0-9]+(?:\.[a-zA-Z0-9-]+)*)?(?:\+[a-zA-Z0-9]+(?:\.[a-zA-Z0-9-]+)*)?$/;

function setupCommander() {
  program
    .option("-n, --packageName <packageName>", "Package name")
    .option(
      "-v, --targetVersion <targetVersion>",
      "Version number of the package"
    );

  program.parse(process.argv);
}

function readPackageJsonFile(path, key, newVersion) {
  const replace = (block, key, newVersion) => {
    const founded = Object.keys(block).filter((x) => x === key);

    if (founded.length > 0) {
      let value = block[key];
      value = value.replace(semverRegex, newVersion);

      return [
        true,
        {
          ...block,
          [key]: value,
        },
      ];
    }

    return [false, block];
  };

  fse.readJson(path, (err, packageObj) => {
    if (err) {
      throw err;
    }

    const { dependencies, peerDependencies, devDependencies } = packageObj;
    const results = [];

    let result = { ...packageObj };
    if (dependencies) {
      const [founded, d] = replace(dependencies, key, newVersion);
      results.push(founded);
      result = {
        ...result,
        dependencies: d,
      };
    }

    if (peerDependencies) {
      const [founded, p] = replace(peerDependencies, key, newVersion);
      results.push(founded);
      result = {
        ...result,
        peerDependencies: p,
      };
    }

    if (devDependencies) {
      const [founded, d] = replace(devDependencies, key, newVersion);
      results.push(founded);
      result = {
        ...result,
        devDependencies: d,
      };
    }

    const anyChanges = !results.some((x) => x);
    if (anyChanges) {
      return;
    }

    console.log("changed", path);
    writeFile(path, result);
  });
}

function writeFile(path, result) {
  return fse.writeJson(path, result, { spaces: 2 });
}

(function findPackageJsonFiles() {
  setupCommander();
  const options = {
    ignore: [
      "../../**/node_modules/**",
      "../../**/dist/**",
      "../../**/build/**",
      "../../**/scripts/**",
      "../../**/wwwroot/**",
    ],
  };

  const workingDir = "../../";
  glob(`${workingDir}**/package.json`, options, (err, files) => {
    if (err) {
      throw err;
    }

    //Todo @masumulu28: check options value and throw error if not provided
    const { packageName, targetVersion } = program.opts();

    for (const file of files) {
      readPackageJsonFile(file, packageName, targetVersion);
    }
  });
})();
