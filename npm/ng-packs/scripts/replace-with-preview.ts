import fse from 'fs-extra';

export default async function(version: string) {
  const corePkgPath = '../packages/core/package.json';
  try {
    const corePkg = await fse.readJSON(corePkgPath);

    await fse.writeJSON(
      corePkgPath,
      {
        ...corePkg,
        dependencies: { ...corePkg.dependencies, '@abp/utils': version },
      },
      { spaces: 2 },
    );
  } catch (error) {
    console.error(error);
  }
}
