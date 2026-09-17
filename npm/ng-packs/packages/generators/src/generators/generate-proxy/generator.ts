import { GenerateProxyGeneratorSchema } from './schema';
import { Tree } from '@nx/devkit';
import { wrapAngularDevkitSchematic } from '@nx/devkit/ngcli-adapter';

export default async function (host: Tree, schema: GenerateProxyGeneratorSchema) {
  const runAngularLibrarySchematic = wrapAngularDevkitSchematic('@abp/ng.schematics', 'proxy-add');

  await runAngularLibrarySchematic(host, {
    ...schema,
  });

  return () => {
    console.log(`proxy added '${schema.target}`);
  };
}
