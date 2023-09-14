import { Tree } from '@nx/devkit';
import { wrapAngularDevkitSchematic } from '@nx/devkit/ngcli-adapter';
import { ChangeThemeGeneratorSchema } from './schema';
import { ThemeOptionsEnum } from './theme-options.enum';

export async function changeThemeGenerator(host: Tree, schema: ChangeThemeGeneratorSchema) {
  const runAngularLibrarySchematic = wrapAngularDevkitSchematic(
    '@abp/ng.schematics',
    'change-theme',
  );

  await runAngularLibrarySchematic(host, {
    ...schema,
  });

  return () => {
    const destTheme = Object.values(ThemeOptionsEnum).find((t, i) => i + 1 === schema.name);
    console.log(`✅️ theme changed to ${destTheme}`);
  };
}

export default changeThemeGenerator;
