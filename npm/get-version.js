const fse = require("fs-extra");

console.log(fse.readJSONSync("package.json").version);
