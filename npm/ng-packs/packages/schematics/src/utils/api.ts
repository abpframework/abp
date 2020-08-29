import { chain, schematic } from '@angular-devkit/schematics';
import { GenerateProxySchema } from '../models';

export function createApisGenerator(schema: GenerateProxySchema, generated: string[]) {
  return chain(
    generated.map(m => {
      const apiSchema = { ...schema, source: schema.target, module: m };
      return schematic('api', apiSchema);
    }),
  );
}
