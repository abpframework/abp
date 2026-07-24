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
  mergeAndAllowDelete,
  removeDefaultPlaceholders,
  resolveProxyResourceApi,
  resolveProject,
} from '../../utils';

export default function (schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = params.module || 'app';

  return async (host: Tree) => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const target = await resolveProject(host, params.target!);
    const targetPath = buildTargetPath(target.definition, params.entryPoint);

    const readProxyConfig = createProxyConfigReader(targetPath);
    const previousConfig = readProxyConfig(host);
    const { generated } = previousConfig;
    const resourceApi = resolveProxyResourceApi(params, previousConfig);

    const index = generated.findIndex(m => m === moduleName);
    if (index < 0) return host;
    generated.splice(index, 1);

    const getApiDefinition = createApiDefinitionGetter(params);
    const data = { generated, resourceApi, ...(await getApiDefinition(host)) };
    data.generated = [];

    const clearProxy = createProxyClearer(targetPath);

    const saveProxyConfig = createProxyConfigSaver(data, targetPath);

    const generateApis = createApisGenerator({ ...schema, resourceApi }, generated);

    const generateIndex = createProxyIndexGenerator(targetPath);

    return chain([
      mergeAndAllowDelete(host, clearProxy),
      saveProxyConfig,
      generateApis,
      generateIndex,
    ]);
  };
}
