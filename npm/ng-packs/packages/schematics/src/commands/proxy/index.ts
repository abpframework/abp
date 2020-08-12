import { chain, SchematicContext, Tree } from '@angular-devkit/schematics';
import { getSourceJson, getSourceUrl, resolveProject } from '../../utils';
import type { Schema as GenerateProxySchema } from './schema';

export default function(params: GenerateProxySchema) {
  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const project = await resolveProject(tree, params.module!);
      const url = params.source || getSourceUrl(tree, project.definition);
      const data = await getSourceJson(url);

      console.log(Object.keys(data.types));
      return chain([]);
    },
  ]);
}
