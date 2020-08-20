import { normalize, strings } from '@angular-devkit/core';
import { applyTemplates, branchAndMerge, chain, move, SchematicContext, SchematicsException, Tree, url } from '@angular-devkit/schematics';
import { Exception } from '../../enums';
import { applyWithOverwrite, buildDefaultPath, createApiDefinitionReader, createControllerToServiceMapper, createImportRefsToModelMapper, createImportRefToEnumMapper, getEnumNamesFromImports, interpolate, resolveProject, serializeParameters } from '../../utils';
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
      const types = data.types;
      const definition = data.modules[moduleName];
      if (!definition) throw new SchematicsException(interpolate(Exception.InvalidModule, moduleName));

      const mapControllerToService = createControllerToServiceMapper(solution, types, definition.remoteServiceName);
      const controllers = Object.values(definition.controllers || {});
      const serviceImports: Record<string, string[]> = {};

      const createServiceFiles = chain(
        controllers.map(controller => {
          const service = mapControllerToService(controller);
          service.imports.forEach(({refs, path}) => refs.forEach(ref => {
            if (path === '@abp/ng.core') return;
            if (!serviceImports[path]) return (serviceImports[path] = [ref]);
            serviceImports[path] = [...new Set([...serviceImports[path], ref])];
          }));

          return applyWithOverwrite(url('./files-service'), [
            applyTemplates({
              ...cases,
              serializeParameters,
              ...service,
            }),
            move(normalize(targetPath)),
          ]);
        }
        ),
      );

      const mapImportRefsToModel = createImportRefsToModelMapper(solution, types);
      const modelImports: Record<string, string[]> = {};

      const createModelFiles = chain(
        Object.values(serviceImports).map(refs => {
          const model = mapImportRefsToModel(refs);
          model.imports.forEach(({refs, path}) => refs.forEach(ref => {
            if (path === '@abp/ng.core') return;
            if (!modelImports[path]) return (modelImports[path] = [ref]);
            modelImports[path] = [...new Set([...modelImports[path], ref])];
          }));

          return applyWithOverwrite(url('./files-model'), [
            applyTemplates({
              ...cases,
              ...model,
            }),
            move(normalize(targetPath)),
          ]);
        }),
      );

      const mapImportRefToEnum = createImportRefToEnumMapper(solution, types);
      const enumRefs = [...new Set([
        ...getEnumNamesFromImports(serviceImports),
        ...getEnumNamesFromImports(modelImports),
      ])];

      const createEnumFiles = chain(
        enumRefs.map(ref => {
          return applyWithOverwrite(url('./files-enum'), [
            applyTemplates({
              ...cases,
              ...mapImportRefToEnum(ref),
            }),
            move(normalize(targetPath)),
          ]);
        }),
      );

      return branchAndMerge(chain([createServiceFiles, createModelFiles, createEnumFiles]));
    },
  ]);
}

