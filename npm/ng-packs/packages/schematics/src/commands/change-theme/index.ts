import { JsonArray, JsonValue } from '@angular-devkit/core';
import { Rule, SchematicsException, Tree, chain } from '@angular-devkit/schematics';
import { ProjectDefinition } from '@angular-devkit/core/src/workspace';
import * as ts from 'typescript';
import { allStyles, importMap, styleMap } from './style-map';
import { ChangeThemeOptions } from './model';
import {
  Change,
  createDefaultPath,
  InsertChange,
  isLibrary,
  updateWorkspace,
  WorkspaceDefinition,
} from '../../utils';
import { ThemeOptionsEnum } from './theme-options.enum';
import {
  addImportToModule,
  findNodes,
  getDecoratorMetadata,
  getMetadataField,
} from '../../utils/angular/ast-utils';

export default function (_options: ChangeThemeOptions): Rule {
  return async () => {
    const targetThemeName = _options.name;
    const selectedProject = _options.targetProject;

    if (!targetThemeName) {
      throw new SchematicsException('The theme name does not selected');
    }

    return chain([
      updateWorkspace(storedWorkspace => {
        updateProjectStyle(selectedProject, storedWorkspace, targetThemeName);
      }),
      updateAppModule(selectedProject, targetThemeName),
    ]);
  };
}

function updateProjectStyle(
  projectName: string,
  workspace: WorkspaceDefinition,
  targetThemeName: ThemeOptionsEnum,
) {
  const project = workspace.projects.get(projectName);

  if (!project) {
    throw new SchematicsException('The target project does not selected');
  }

  const isProjectLibrary = isLibrary(project);
  if (isProjectLibrary) {
    throw new SchematicsException('The library project does not supported');
  }

  const targetOption = getProjectTargetOptions(project, 'build');
  const styles = targetOption.styles as (string | { input: string })[];

  const sanitizedStyles = removeThemeBasedStyles(styles);

  const newStyles = styleMap.get(targetThemeName);
  if (!newStyles) {
    throw new SchematicsException('The theme does not found');
  }
  targetOption.styles = [...newStyles, ...sanitizedStyles] as JsonArray;
}

function updateAppModule(selectedProject: string, targetThemeName: ThemeOptionsEnum): Rule {
  return async (host: Tree) => {
    const appModulePath = (await createDefaultPath(host, selectedProject)) + '/app.module.ts';

    return chain([
      removeImportPath(appModulePath, targetThemeName),
      removeImportFromNgModuleMetadata(appModulePath, targetThemeName),
      insertImports(appModulePath, targetThemeName),
    ]);
  };
}

export function removeImportPath(appModulePath: string, selectedTheme: ThemeOptionsEnum): Rule {
  return (host: Tree) => {
    const recorder = host.beginUpdate(appModulePath);
    const source = createSourceFile(host, appModulePath);
    const impMap = getImportPaths(selectedTheme, true);

    const nodes = findNodes(source, ts.isImportDeclaration);

    const filteredNodes = nodes.filter(node =>
      impMap.some(({ path, importName }) => {
        const sourceModule = node.getFullText();
        const moduleName = importName.split('.')[0];

        if (path && sourceModule.match(path)) {
          return true;
        }

        return !!(moduleName && sourceModule.match(moduleName));
      }),
    );

    if (filteredNodes?.length < 1) {
      return;
    }

    filteredNodes.map(importPath =>
      recorder.remove(importPath.getStart(), importPath.getWidth() + 1),
    );

    host.commitUpdate(recorder);
    return host;
  };
}

export function removeImportFromNgModuleMetadata(
  appModulePath: string,
  selectedTheme: ThemeOptionsEnum,
): Rule {
  return (host: Tree) => {
    const recorder = host.beginUpdate(appModulePath);
    const source = createSourceFile(host, appModulePath);
    const impMap = getImportPaths(selectedTheme, true);

    const node = getDecoratorMetadata(source, 'NgModule', '@angular/core')[0] || {};
    if (!node) {
      throw new SchematicsException('The app module does not found');
    }

    const matchingProperties = getMetadataField(node as ts.ObjectLiteralExpression, 'imports');
    const assignment = matchingProperties[0] as ts.PropertyAssignment;
    const assignmentInit = assignment.initializer as ts.ArrayLiteralExpression;

    const elements = assignmentInit.elements;
    if (!elements || elements.length < 1) {
      throw new SchematicsException(`Elements could not found: ${elements}`);
    }

    const filteredElements = elements.filter(f =>
      impMap.some(s => f.getText().match(s.importName)),
    );

    if (!filteredElements || filteredElements.length < 1) {
      return;
    }

    filteredElements.map(willRemoveModule =>
      recorder.remove(willRemoveModule.getStart(), willRemoveModule.getWidth() + 1),
    );
    host.commitUpdate(recorder);
    return host;
  };
}

export function insertImports(appModulePath: string, selectedTheme: ThemeOptionsEnum): Rule {
  return (host: Tree) => {
    const recorder = host.beginUpdate(appModulePath);
    const source = createSourceFile(host, appModulePath);
    const selected = importMap.get(selectedTheme);

    if (!selected) {
      return host;
    }

    const changes: Change[] = [];

    selected.map(({ importName, path }) =>
      changes.push(...addImportToModule(source, appModulePath, importName, path)),
    );

    if (changes.length > 0) {
      for (const change of changes) {
        if (change instanceof InsertChange) {
          recorder.insertLeft(change.pos, change.toAdd);
        }
      }
    }
    host.commitUpdate(recorder);
    return host;
  };
}

export function createSourceFile(host: Tree, appModulePath: string): ts.SourceFile {
  const buffer = host.read(appModulePath);
  if (!buffer || buffer.length === 0) {
    throw new SchematicsException(`${appModulePath} file could not be read.`);
  }

  const sourceText = buffer.toString('utf-8');

  return ts.createSourceFile(
    appModulePath,
    sourceText,
    ts.ScriptTarget.Latest,
    true,
    ts.ScriptKind.TS,
  );
}

/**
 * Returns all import paths except the selected theme
 * @param selectedTheme The selected theme
 * @param getAll If true, returns all import paths
 */
export function getImportPaths(selectedTheme: ThemeOptionsEnum, getAll: boolean = false) {
  if (getAll) {
    return Array.from(importMap.values()).reduce((acc, val) => [...acc, ...val], []);
  }

  return Array.from(importMap.values())
    .filter(f => f !== importMap.get(selectedTheme))
    .reduce((acc, val) => [...acc, ...val], []);
}

export function getProjectTargetOptions(
  project: ProjectDefinition,
  buildTarget: string,
): Record<string, JsonValue | undefined> {
  const options = project.targets?.get(buildTarget)?.options;

  if (!options) {
    throw new SchematicsException(
      `Cannot determine project target configuration for: ${buildTarget}.`,
    );
  }

  return options;
}

export function removeThemeBasedStyles(styles: (string | object)[]) {
  return styles.filter(s => !allStyles.some(x => styleCompareFn(s, x)));
}

export const styleCompareFn = (item1: string | object, item2: string | object) => {
  const type1 = typeof item1;
  const type2 = typeof item1;

  if (type1 !== type2) {
    return false;
  }

  if (type1 === 'string') {
    return item1 === item2;
  }
  const o1 = item1 as { bundleName?: string };
  const o2 = item2 as { bundleName?: string };

  return o1.bundleName && o2.bundleName && o1.bundleName == o2.bundleName;
};
