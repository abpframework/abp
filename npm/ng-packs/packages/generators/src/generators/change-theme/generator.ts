import { Tree } from '@nx/devkit';
import { wrapAngularDevkitSchematic } from '@nx/devkit/ngcli-adapter';
import { ChangeThemeGeneratorSchema } from './schema';
import { ThemeOptionsEnum } from './theme-options.enum';

export async function changeThemeGenerator(host: Tree, schema: ChangeThemeGeneratorSchema) {
  const schematicPath = schema.localPath || '@abp/ng.schematics';

  const runAngularLibrarySchematic = wrapAngularDevkitSchematic(
    schema.localPath ? `${host.root}${schematicPath}` : schematicPath,
    'change-theme',
  );

  await runAngularLibrarySchematic(host, {
    ...schema,
  });

  return () => {
    const destTheme = Object.values(ThemeOptionsEnum).find(
      (theme, index) => index + 1 === schema.name,
    );
    console.log(`✅️ Switched to Theme ${destTheme}`);
  };
}

export default changeThemeGenerator;
