import { strings } from '@angular-devkit/core';
import { chain, SchematicContext, Tree } from '@angular-devkit/schematics';
import { API_DEFINITION_ENDPOINT } from '../../constants';
import { ApiDefinition } from '../../models';
import { getSourceJson, getSourceUrl, resolveProject } from '../../utils';
import type { Schema as GenerateProxySchema } from './schema';

export default function(params: GenerateProxySchema) {
  const moduleName = strings.camelize(params.module || 'app');

  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const source = await resolveProject(tree, params.source!);
      const url = getSourceUrl(tree, source, moduleName);
      const data: ApiDefinition = await getSourceJson(url + API_DEFINITION_ENDPOINT);

      const services = Object.entries(data.modules).map(([name, def]) => [
        name,
        Object.values(def.controllers).map(
          ({controllerName, actions}) => [controllerName, Object.keys(actions)]
        ),
      ]);

      const defs = services.filter(([name]) => name === moduleName);

      console.log(defs);
      return chain([]);
    },
  ]);
}
