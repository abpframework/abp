import { normalize, strings } from '@angular-devkit/core';
import {
  applyTemplates,
  branchAndMerge,
  chain,
  move,
  SchematicContext,
  SchematicsException,
  Tree,
  url,
} from '@angular-devkit/schematics';
import { Exception } from '../../enums';
import { ServiceGeneratorParams } from '../../models';
import {
  applyWithOverwrite,
  buildDefaultPath,
  createApiDefinitionReader,
  createControllerToServiceMapper,
  createImportRefsToModelReducer,
  createImportRefToEnumMapper,
  EnumGeneratorParams,
  getEnumNamesFromImports,
  getRootNamespace,
  interpolate,
  ModelGeneratorParams,
  removeDefaultPlaceholders,
  resolveProject,
  serializeParameters,
} from '../../utils';
import * as cases from '../../utils/text';
import { Schema as GenerateProxySchema } from './schema';

export default function(schema: GenerateProxySchema) {
  const params = removeDefaultPlaceholders(schema);
  const moduleName = strings.camelize(params.module || 'app');

  return chain([
    async (tree: Tree, _context: SchematicContext) => {
      const source = await resolveProject(tree, params.source!);
      const target = await resolveProject(tree, params.target!);
      const solution = getRootNamespace(tree, source, moduleName);
      const targetPath = buildDefaultPath(target.definition);
      const definitionPath = `${targetPath}/shared/api-definition.json`;
      const readApiDefinition = createApiDefinitionReader(definitionPath);
      const data = readApiDefinition(tree);
      const types = data.types;
      const modules = data.modules;
      if (!types || !modules) throw new SchematicsException(Exception.InvalidApiDefinition);

      const definition = data.modules[moduleName];
      if (!definition)
        throw new SchematicsException(interpolate(Exception.InvalidModule, moduleName));

      const apiName = definition.remoteServiceName;
      const controllers = Object.values(definition.controllers || {});
      const serviceImports: Record<string, string[]> = {};
      const generateServices = createServiceGenerator({
        targetPath,
        solution,
        types,
        apiName,
        controllers,
        serviceImports,
      });

      const modelImports: Record<string, string[]> = {};
      const generateModels = createModelGenerator({
        targetPath,
        solution,
        types,
        serviceImports,
        modelImports,
      });

      const generateEnums = createEnumGenerator({
        targetPath,
        solution,
        types,
        serviceImports,
        modelImports,
      });

      return branchAndMerge(chain([generateServices, generateModels, generateEnums]));
    },
  ]);
}

function createEnumGenerator(params: EnumGeneratorParams) {
  const { targetPath, serviceImports, modelImports } = params;
  const mapImportRefToEnum = createImportRefToEnumMapper(params);
  const enumRefs = [
    ...new Set([
      ...getEnumNamesFromImports(serviceImports),
      ...getEnumNamesFromImports(modelImports),
    ]),
  ];

  return chain(
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
}

function createModelGenerator(params: ModelGeneratorParams) {
  const { targetPath, serviceImports, modelImports } = params;
  const reduceImportRefsToModels = createImportRefsToModelReducer(params);
  const models = Object.values(serviceImports).reduce(reduceImportRefsToModels, []);
  models.forEach(({ imports }) =>
    imports.forEach(({ refs, path }) =>
      refs.forEach(ref => {
        if (path === '@abp/ng.core') return;
        if (!modelImports[path]) return (modelImports[path] = [ref]);
        modelImports[path] = [...new Set([...modelImports[path], ref])];
      }),
    ),
  );

  return chain(
    models.map(model =>
      applyWithOverwrite(url('./files-model'), [
        applyTemplates({
          ...cases,
          ...model,
        }),
        move(normalize(targetPath)),
      ]),
    ),
  );
}

function createServiceGenerator(params: ServiceGeneratorParams) {
  const { targetPath, controllers, serviceImports } = params;
  const mapControllerToService = createControllerToServiceMapper(params);

  return chain(
    controllers.map(controller => {
      const service = mapControllerToService(controller);
      service.imports.forEach(({ refs, path }) =>
        refs.forEach(ref => {
          if (path === '@abp/ng.core') return;
          if (!serviceImports[path]) return (serviceImports[path] = [ref]);
          serviceImports[path] = [...new Set([...serviceImports[path], ref])];
        }),
      );

      return applyWithOverwrite(url('./files-service'), [
        applyTemplates({
          ...cases,
          serializeParameters,
          ...service,
        }),
        move(normalize(targetPath)),
      ]);
    }),
  );
}
