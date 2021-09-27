import { chain, SchematicContext, Tree } from '@angular-devkit/schematics';
import { GenerateProxySchema } from '../../models';
import {
  buildDefaultPath,
  createApiDefinitionGetter,
  createApisGenerator,
  createProxyClearer,
  createProxyConfigReader,
  createProxyConfigSaver,
  createProxyIndexGenerator,
  mergeAndAllowDelete,
  removeDefaultPlaceholders,
  resolveProject,
} from '../../utils';

export default function (schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = params.module || 'app';

  return async (host: Tree, _context: SchematicContext) => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const target = await resolveProject(host, params.target!);
    const targetPath = buildDefaultPath(target.definition);

    const readProxyConfig = createProxyConfigReader(targetPath);
    const { generated } = readProxyConfig(host);

    const index = generated.findIndex(m => m === moduleName);
    if (index < 0) return host;
    generated.splice(index, 1);

    const getApiDefinition = createApiDefinitionGetter(params);
    const data = { generated, ...(await getApiDefinition(host)) };
    data.generated = [];

    const clearProxy = createProxyClearer(targetPath);

    const saveProxyConfig = createProxyConfigSaver(data, targetPath);

    const generateApis = createApisGenerator(schema, generated);

    const generateIndex = createProxyIndexGenerator(targetPath);

    return chain([
      mergeAndAllowDelete(host, clearProxy),
      saveProxyConfig,
      generateApis,
      generateIndex,
    ]);
  };
}
