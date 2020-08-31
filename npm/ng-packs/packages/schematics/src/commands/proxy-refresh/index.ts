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

export default function(schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);

  return async (host: Tree, _context: SchematicContext) => {
    const target = await resolveProject(host, params.target!);
    const targetPath = buildDefaultPath(target.definition);

    const readProxyConfig = createProxyConfigReader(targetPath);
    const { generated } = readProxyConfig(host);

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
