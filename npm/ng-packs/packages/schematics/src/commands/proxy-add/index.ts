import { chain, Tree } from '@angular-devkit/schematics';
import { GenerateProxySchema } from '../../models';
import {
  buildTargetPath,
  createApiDefinitionGetter,
  createApisGenerator,
  createProxyClearer,
  createProxyConfigReader,
  createProxyConfigSaver,
  createProxyIndexGenerator,
  createProxyWarningSaver,
  mergeAndAllowDelete,
  removeDefaultPlaceholders,
  resolveProxyResourceApi,
  resolveProject,
} from '../../utils';

export default function (schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = params.module || 'app';

  return chain([
    async (host: Tree) => {
      const target = await resolveProject(host, params.target!);
      const targetPath = buildTargetPath(target.definition, params.entryPoint);
      const readProxyConfig = createProxyConfigReader(targetPath);
      let generated: string[] = [];
      let previousResourceApi = false;

      try {
        const previousConfig = readProxyConfig(host);
        generated = previousConfig.generated;
        previousResourceApi = resolveProxyResourceApi(params, previousConfig);
        const index = generated.findIndex(m => m === moduleName);
        if (index < 0) generated.push(moduleName);
      } catch (_) {
        generated.push(moduleName);
        previousResourceApi = resolveProxyResourceApi(params);
      }

      const getApiDefinition = createApiDefinitionGetter(params);
      const data = { generated, resourceApi: previousResourceApi, ...(await getApiDefinition(host)) };
      data.generated = [];

      const clearProxy = createProxyClearer(targetPath);

      const saveProxyConfig = createProxyConfigSaver(data, targetPath);

      const saveProxyWarning = createProxyWarningSaver(targetPath);

      const generateApis = createApisGenerator({ ...schema, resourceApi: previousResourceApi }, generated);

      const generateIndex = createProxyIndexGenerator(targetPath);

      console.log('HELLO');

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
