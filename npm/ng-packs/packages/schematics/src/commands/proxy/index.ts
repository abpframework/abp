import { normalize, strings } from '@angular-devkit/core';
import { applyTemplates, branchAndMerge, chain, move, SchematicContext, Tree, url } from '@angular-devkit/schematics';
import { API_DEFINITION_ENDPOINT } from '../../constants';
import { ApiDefinition } from '../../models';
import { applyWithOverwrite, buildDefaultPath, getSourceJson, getSourceUrl, isLibrary, resolveProject } from '../../utils';
import * as cases from '../../utils/text';
import type { Schema as GenerateProxySchema } from './schema';

export default function(params: GenerateProxySchema) {
  const moduleName = strings.camelize(params.module || 'app');

  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const source = await resolveProject(tree, params.source!);
      const target = await resolveProject(tree, params.target!);
      const isModule = isLibrary(target.definition);
      const sourceUrl = getSourceUrl(tree, source, moduleName);
      const targetPath = buildDefaultPath(target.definition);
      const data: ApiDefinition = await getSourceJson(sourceUrl + API_DEFINITION_ENDPOINT);

      const controllers = Object.values(data.modules[moduleName]?.controllers || {});

      const createServiceFiles = chain(
        controllers.map(controller => {
          const {type} = controller;
          const [namespace] = type.replace(/^Volo\.(Abp\.)?/, '').split('.');

          return applyWithOverwrite(url('./files-service'), [
            applyTemplates({
              ...cases,
              name: controller.type.split('.').pop()!.replace('Controller', ''),
              sharedPath: 'shared/' + strings.dasherize(namespace),
              controller,
            }),
            move(normalize(targetPath)),
          ]);
        }
        ),
      );

      console.log(isModule);

      return branchAndMerge(chain([createServiceFiles]));
    },
  ]);
}
