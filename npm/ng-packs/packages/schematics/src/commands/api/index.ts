import { normalize, strings } from '@angular-devkit/core';
import { applyTemplates, branchAndMerge, chain, move, SchematicContext, Tree, url } from '@angular-devkit/schematics';
import { applyWithOverwrite, buildDefaultPath, createApiDefinitionReader, resolveProject } from '../../utils';
import * as cases from '../../utils/text';
import type { Schema as GenerateProxySchema } from './schema';

export default function(params: GenerateProxySchema) {
  const solution = params.solution;
  const moduleName = strings.camelize(params.module || 'app');

  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const target = await resolveProject(tree, params.target!);
      const targetPath = buildDefaultPath(target.definition);
      const readApiDefinition = createApiDefinitionReader(`${targetPath}/shared/api-definition.json`);
      const data = readApiDefinition(tree);

      const controllers = Object.values(data.modules[moduleName]?.controllers || {});

      const createServiceFiles = chain(
        controllers.map(controller => {
          const {controllerName: name, type, actions, interfaces} = controller;
          let namespace = type.split('.').slice(0, -1).join('.');
          solution.split('.').reduceRight((acc, part) => {
            acc = part + '\.' + acc;
            const regex = new RegExp('^' + acc + '(Controllers\.)?');
            namespace = namespace.replace(regex, '');
            return acc;
          }, '');

          console.log(namespace);

          return applyWithOverwrite(url('./files-service'), [
            applyTemplates({
              ...cases,
              solution,
              namespace,
              name,
              apiName: data.modules[moduleName].remoteServiceName,
              apiUrl: controller.actions[0]?.url,
              actions,
              interfaces,
            }),
            move(normalize(targetPath)),
          ]);
        }
        ),
      );

      return branchAndMerge(chain([createServiceFiles]));
    },
  ]);
}
