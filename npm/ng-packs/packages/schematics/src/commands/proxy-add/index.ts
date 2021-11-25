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
  createProxyWarningSaver,
  mergeAndAllowDelete,
  removeDefaultPlaceholders,
  resolveProject,
} from '../../utils';

export default function (schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = params.module || 'app';

  return chain([
    async (host: Tree, _context: SchematicContext) => {
      const target = await resolveProject(host, params.target!);
      const targetPath = buildDefaultPath(target.definition);
      const readProxyConfig = createProxyConfigReader(targetPath);
      let generated: string[] = [];

      try {
        generated = readProxyConfig(host).generated;
        const index = generated.findIndex(m => m === moduleName);
        if (index < 0) generated.push(moduleName);
      } catch (_) {
        generated.push(moduleName);
      }

      const getApiDefinition = createApiDefinitionGetter(params);
      const data = { generated, ...(await getApiDefinition(host)) };
      data.generated = [];

      const clearProxy = createProxyClearer(targetPath);

      const saveProxyConfig = createProxyConfigSaver(data, targetPath);

      const saveProxyWarning = createProxyWarningSaver(targetPath);

      const generateApis = createApisGenerator(schema, generated);

      const generateIndex = createProxyIndexGenerator(targetPath);

      return chain([
        mergeAndAllowDelete(host, clearProxy),
        saveProxyConfig,
        saveProxyWarning,
        generateApis,
        generateIndex,
      ]);
    },
  ]);
}
