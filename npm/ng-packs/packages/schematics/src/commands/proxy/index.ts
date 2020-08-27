import { strings } from '@angular-devkit/core';
import {
  branchAndMerge,
  chain,
  schematic,
  SchematicContext,
  Tree,
} from '@angular-devkit/schematics';
import { API_DEFINITION_ENDPOINT } from '../../constants';
import { ApiDefinition } from '../../models';
import {
  buildDefaultPath,
  createApiDefinitionSaver,
  getApiDefinition,
  getSourceUrl,
  removeDefaultPlaceholders,
  resolveProject,
} from '../../utils';
import { Schema as GenerateProxySchema } from './schema';

export default function(schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = strings.camelize(params.module || 'app');

  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const source = await resolveProject(tree, params.source!);
      const target = await resolveProject(tree, params.target!);
      const sourceUrl = getSourceUrl(tree, source, moduleName);
      const targetPath = buildDefaultPath(target.definition);
      const data: ApiDefinition = await getApiDefinition(sourceUrl + API_DEFINITION_ENDPOINT);

      const saveApiDefinition = createApiDefinitionSaver(
        data,
        `${targetPath}/shared/api-definition.json`,
      );
      const createApi = schematic('api', schema);

      return branchAndMerge(chain([saveApiDefinition, createApi]));
    },
  ]);
}
