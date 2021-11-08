const glob = require("glob");
const fse = require("fs-extra");

function replace(filePath) {
  const pkg = fse.readJsonSync(filePath);

  const { dependencies } = pkg;

  if (!dependencies) return;

  Object.keys(dependencies).forEach((key) => {
    if (key.includes("@abp/")) {
      dependencies[key] = dependencies[key].replace("^", "~");
    }
  });

  fse.writeJsonSync(filePath, { ...pkg, dependencies }, { spaces: 2 });
}

glob("./packs/**/package.json", {}, (er, files) => {
  files.forEach((path) => {
    if (path.includes("node_modules")) {
      return;
    }

    replace(path);
  });
});