const { resolve } = require("path");
const { readdir, rmdir } = require("fs").promises;

async function* removeFolder(dir) {
  await rmdir(dir, { recursive: true });
  yield dir;
}

async function* getBinObj(dir) {
  const dirents = await readdir(dir, { withFileTypes: true });

  for (const dirent of dirents) {
    if (!dirent.isDirectory()) continue;
    const name = dirent.name;

    if (name === "node_modules") continue;

    const res = resolve(dir, dirent.name);

    if (name === "bin" || name === "obj") {
      yield* removeFolder(res);
      continue;
    }

    yield* getBinObj(res);
  }
}

(async () => {
  console.log("\x1b[36m%s\x1b[0m", "Deleting all BIN and OBJ folders...");

  for await (const dir of getBinObj(".")) {
    console.log("\x1b[33m%s\x1b[0m", `Removed: ${dir}`);
  }

  console.log("\x1b[36m%s\x1b[0m", "All BIN and OBJ folders are deleted.");
})();
