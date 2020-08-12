import { chain, SchematicContext, Tree } from '@angular-devkit/schematics';
import { Schema as GenerateProxySchema } from './schema';

export default function(_params: GenerateProxySchema) {
  return chain([
    async (_tree: Tree, _context: SchematicContext) => {
      return chain([]);
    },
  ]);
}
