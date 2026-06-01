import { JsonArray, JsonValue } from '@angular-devkit/core';
import { Rule, SchematicsException, Tree, chain } from '@angular-devkit/schematics';
import { ProjectDefinition } from '@angular-devkit/core/src/workspace';
import * as ts from 'typescript';
import { allStyles, importMap, styleMap } from './style-map';
import { ChangeThemeOptions } from './model';
import {
  addRootImport,
  addRootProvider,
  getAppModulePath,
  isLibrary,
  isStandaloneApp,
  updateWorkspace,
  WorkspaceDefinition,
  getAppConfigPath,
  cleanEmptyExprFromModule,
  cleanEmptyExprFromProviders,
  getWorkspace,
} from '../../utils';
import { ThemeOptionsEnum } from './theme-options.enum';
import { findNodes, getDecoratorMetadata, getMetadataField } from '../../utils/angular/ast-utils';
import { getMainFilePath } from '../../utils/angular/standalone/util';

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

  if (isLibrary(project)) {
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
    const mainFilePath = await getMainFilePath(host, selectedProject);
    const isStandalone = isStandaloneApp(host, mainFilePath);
    const appModulePath = isStandalone
      ? getAppConfigPath(host, mainFilePath)
      : getAppModulePath(host, mainFilePath);

    return chain([
      removeImportPath(appModulePath, targetThemeName),
      ...(!isStandalone ? [removeImportFromNgModuleMetadata(appModulePath, targetThemeName)] : []),
      isStandalone
        ? removeImportsFromStandaloneProviders(appModulePath, targetThemeName)
        : removeProviderFromNgModuleMetadata(appModulePath, targetThemeName),
      insertHelperImports(appModulePath, targetThemeName),
      insertImports(selectedProject, targetThemeName),
      insertProviders(selectedProject, targetThemeName),
      adjustProvideAbpThemeShared(appModulePath, targetThemeName),
      updateIndexHtml(selectedProject, targetThemeName),
      cleanEmptyExpressions(appModulePath, isStandalone),
    ]);
  };
}

export function removeImportPath(filePath: string, selectedTheme: ThemeOptionsEnum): Rule {
  return (host: Tree) => {
    const buffer = host.read(filePath);
    if (!buffer) return host;

    const sourceText = buffer.toString('utf-8');
    const source = ts.createSourceFile(filePath, sourceText, ts.ScriptTarget.Latest, true);
    const recorder = host.beginUpdate(filePath);

    const impMap = getImportPaths(selectedTheme, true);

    const nodes = findNodes(source, ts.isImportDeclaration);

    const filteredNodes = nodes.filter(node => {
      const importPath = (node.moduleSpecifier as ts.StringLiteral).text;
      const namedBindings = node.importClause?.namedBindings;

      return impMap.some(({ path, importName }) => {
        const symbol = importName.split('.')[0];
        const matchesPath = !!path && importPath === path;

        const matchesSymbol =
          !!namedBindings &&
          ts.isNamedImports(namedBindings) &&
          namedBindings.elements.some(e => e.name.text === symbol);

        return matchesPath || matchesSymbol;
      });
    });

    for (const node of filteredNodes) {
      recorder.remove(node.getStart(), node.getWidth());
    }

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

export function removeImportsFromStandaloneProviders(
  mainPath: string,
  selectedTheme: ThemeOptionsEnum,
): Rule {
  return (host: Tree) => {
    const buffer = host.read(mainPath);
    if (!buffer) return host;

    const sourceText = buffer.toString('utf-8');
    const source = ts.createSourceFile(mainPath, sourceText, ts.ScriptTarget.Latest, true);
    const recorder = host.beginUpdate(mainPath);

    const impMap = getImportPaths(selectedTheme, true);
    const callExpressions = findNodes(source, ts.isCallExpression);

    for (const expr of callExpressions) {
      const exprText = expr.getText();

      if (expr.expression.getText() === 'importProvidersFrom') {
        const args = expr.arguments;

        let modules: readonly ts.Expression[] = [];

        if (args.length === 1 && ts.isArrayLiteralExpression(args[0])) {
          modules = (args[0] as ts.ArrayLiteralExpression).elements;
        } else {
          modules = args;
        }

        const elementsToRemove = modules.filter(el =>
          impMap.some(({ importName }) => el.getText().includes(importName)),
        );

        if (elementsToRemove.length) {
          for (const removeEl of elementsToRemove) {
            const start = removeEl.getFullStart();
            const end = removeEl.getEnd();

            const nextChar = sourceText.slice(end, end + 1);
            const prevChar = sourceText.slice(start - 1, start);

            if (nextChar === ',') {
              recorder.remove(start, end - start + 1);
            } else if (prevChar === ',') {
              recorder.remove(start - 1, end - start + 1);
            } else {
              recorder.remove(start, end - start);
            }
          }
        }

        const remaining = modules.filter(el => !elementsToRemove.includes(el));
        if (remaining.length === 0) {
          const start = expr.getFullStart();
          const end = expr.getEnd();
          const nextChar = sourceText.slice(end, end + 1);
          const prevChar = sourceText.slice(start - 1, start);

          if (nextChar === ',') {
            recorder.remove(start, end - start + 1);
          } else if (prevChar === ',') {
            recorder.remove(start - 1, end - start + 1);
          } else {
            recorder.remove(start, end - start);
          }
        }
      } else {
        const match = impMap.find(({ importName, provider }) => {
          const moduleSymbol = importName?.split('.')[0];
          return (
            (moduleSymbol && exprText.includes(moduleSymbol)) ||
            (provider && exprText.includes(provider))
          );
        });

        if (match) {
          const start = expr.getFullStart();
          const end = expr.getEnd();
          const nextChar = sourceText.slice(end, end + 1);
          const prevChar = sourceText.slice(start - 1, start);

          if (nextChar === ',') {
            recorder.remove(start, end - start + 1);
          } else if (prevChar === ',') {
            recorder.remove(start - 1, end - start + 1);
          } else {
            recorder.remove(start, end - start);
          }
        }
      }
    }

    host.commitUpdate(recorder);
    return host;
  };
}

export function removeProviderFromNgModuleMetadata(
  appModulePath: string,
  selectedTheme: ThemeOptionsEnum,
): Rule {
  return (host: Tree) => {
    const recorder = host.beginUpdate(appModulePath);
    const source = createSourceFile(host, appModulePath);
    const impMap = getImportPaths(selectedTheme, true);

    const node = getDecoratorMetadata(source, 'NgModule', '@angular/core')[0];
    if (!node) {
      throw new SchematicsException('The app module does not found');
    }

    const providersProperty = getMetadataField(
      node as ts.ObjectLiteralExpression,
      'providers',
    )[0] as ts.PropertyAssignment;

    const providersArray = providersProperty.initializer as ts.ArrayLiteralExpression;
    if (!providersArray.elements.length) return host;

    for (const element of providersArray.elements) {
      const elementText = element.getText();

      const match = impMap.find(({ provider }) => {
        if (!provider) return false;
        const providerName = provider.replace(/\(\s*\)$/, '').trim();
        return provider && elementText.includes(providerName);
      });

      if (match) {
        const start = element.getFullStart();
        const end = element.getEnd();

        const nextChar = source.text.slice(end, end + 1);
        const prevChar = source.text.slice(start - 1, start);

        if (nextChar === ',') {
          recorder.remove(start, end - start + 1);
        } else if (prevChar === ',') {
          recorder.remove(start - 1, end - start + 1);
        } else {
          recorder.remove(start, end - start);
        }
      }
    }

    host.commitUpdate(recorder);
    return host;
  };
}

export function insertHelperImports(filePath: string, selectedTheme: ThemeOptionsEnum): Rule {
  return (host: Tree) => {
    const selectedThemeImports = importMap.get(selectedTheme);
    const helpers = selectedThemeImports?.filter(
      s => !s.doNotImport && s.importName && s.path && !s.expression && !s.provider
    );
    
    if (!helpers || helpers.length === 0) {
      return host;
    }

    const buffer = host.read(filePath);
    if (!buffer) return host;

    const sourceText = buffer.toString('utf-8');
    const source = ts.createSourceFile(filePath, sourceText, ts.ScriptTarget.Latest, true);
    const recorder = host.beginUpdate(filePath);

    for (const { importName, path } of helpers) {
      const existingImport = findNodes(source, ts.isImportDeclaration).find(node => {
        const moduleSpecifier = (node.moduleSpecifier as ts.StringLiteral).text;
        const namedBindings = node.importClause?.namedBindings;
        
        if (moduleSpecifier === path && namedBindings && ts.isNamedImports(namedBindings)) {
          return namedBindings.elements.some(e => e.name.text === importName);
        }
        return false;
      });

      if (!existingImport) {
        const importStatement = `import { ${importName} } from '${path}';\n`;
        recorder.insertLeft(0, importStatement);
      }
    }

    host.commitUpdate(recorder);
    return host;
  };
}

export function insertImports(projectName: string, selectedTheme: ThemeOptionsEnum): Rule {
  const selectedThemeImports = importMap.get(selectedTheme);
  const selected = selectedThemeImports?.filter(s => !s.doNotImport && !!s.expression);
  
  if (!selected || selected.length === 0) {
    return () => {};
  }

  return addRootImport(projectName, code => {
    const expressions = selected.map(({ importName, path, expression }) => {
      if (importName && path) {
        code.external(importName, path);
      }
      return expression!.trim();
    });

    return code.code`
${expressions.join(',\n')}`;
  });
}
export function insertProviders(projectName: string, selectedTheme: ThemeOptionsEnum): Rule {
  const selectedThemeImports = importMap.get(selectedTheme);
  const selected = selectedThemeImports?.filter(s => !s.doNotImport && !!s.provider);
  
  if (!selected || selected.length === 0) {
    return () => {};
  }

  return addRootProvider(projectName, code => {
    const providers = selected.map(({ provider, path, importName }) => {
      code.external(importName, path);
      return provider;
    });

    return code.code`
${providers.join(',\n')}`;
  });
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
export function getImportPaths(selectedTheme: ThemeOptionsEnum, getAll = false) {
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

export const formatFile = (filePath: string): Rule => {
  return (tree: Tree) => {
    const buffer = tree.read(filePath);
    if (!buffer) return tree;

    const source = ts.createSourceFile(filePath, buffer.toString(), ts.ScriptTarget.Latest, true);
    const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed });
    const formatted = printer.printFile(source);

    tree.overwrite(filePath, formatted);
    return tree;
  };
};

export function cleanEmptyExpressions(modulePath: string, isStandalone: boolean): Rule {
  return (host: Tree) => {
    const buffer = host.read(modulePath);
    if (!buffer) throw new SchematicsException(`Cannot read ${modulePath}`);

    const source = ts.createSourceFile(
      modulePath,
      buffer.toString('utf-8'),
      ts.ScriptTarget.Latest,
      true,
    );
    const recorder = host.beginUpdate(modulePath);

    if (isStandalone) {
      cleanEmptyExprFromProviders(source, recorder);
    } else {
      cleanEmptyExprFromModule(source, recorder);
    }
    host.commitUpdate(recorder);
    return host;
  };
}

export function adjustProvideAbpThemeShared(
  appModulePath: string,
  selectedTheme: ThemeOptionsEnum,
): Rule {
  return (host: Tree) => {
    const source = createSourceFile(host, appModulePath);
    const recorder = host.beginUpdate(appModulePath);
    const sourceText = source.getText();

    const callExpressions = findProvideAbpThemeSharedCalls(source);

    for (const expr of callExpressions) {
      const exprStart = expr.getStart();
      const exprEnd = expr.getEnd();
      const originalText = sourceText.substring(exprStart, exprEnd);

      let newText = originalText;

      const hasHttpErrorConfig = originalText.includes('withHttpErrorConfig');
      const hasValidationBluePrint = originalText.includes('withValidationBluePrint');

      if (selectedTheme === ThemeOptionsEnum.LeptonX) {
        if (!hasHttpErrorConfig) {
          newText = newText.replace(
            '(',
            `(
  withHttpErrorConfig({
    errorScreen: {
      component: HttpErrorComponent,
      forWhichErrors: [401, 403, 404, 500],
      hideCloseIcon: true
    }
  }),`,
          );
        }
      } else {
        if (hasHttpErrorConfig) {
          newText = newText.replace(/withHttpErrorConfig\([^)]*\),?/, '');
        }
      }

      if (!hasValidationBluePrint) {
        newText = newText.replace(
          '(',
          `(
  withValidationBluePrint({
    wrongPassword: 'Please choose 1q2w3E*'
  }),`,
        );
      }

      if (newText && newText !== originalText) {
        recorder.remove(exprStart, exprEnd - exprStart);
        recorder.insertLeft(exprStart, newText);
      }
    }

    host.commitUpdate(recorder);
    return host;
  };
}

function findProvideAbpThemeSharedCalls(source: ts.SourceFile): ts.CallExpression[] {
  const result: ts.CallExpression[] = [];

  const visit = (node: ts.Node) => {
    if (ts.isCallExpression(node)) {
      const expressionText = node.expression.getText();
      if (expressionText.includes('provideAbpThemeShared')) {
        result.push(node);
      }
    }
    ts.forEachChild(node, visit);
  };

  visit(source);

  return result;
}

export function updateIndexHtml(projectName: string, themeName: ThemeOptionsEnum): Rule {
  return async (host: Tree) => {
    const workspace = await getWorkspace(host);
    const project = workspace.projects.get(projectName);

    if (!project) {
      throw new Error(`Project "${projectName}" not found in workspace.`);
    }

    const buildOptions = project.targets.get('build')?.options;
    const indexPath = buildOptions?.index as string;

    if (!indexPath || !host.exists(indexPath)) {
      throw new Error(`index.html not found at path: ${indexPath}`);
    }

    const buffer = host.read(indexPath);
    if (!buffer) return;
    const content = buffer.toString('utf-8');

    const loaderDiv = `<div id="lp-page-loader"></div>`;
    let updatedContent = content;

    if (themeName === ThemeOptionsEnum.LeptonX) {
      if (!content.includes(loaderDiv)) {
        updatedContent = content.replace(/<body([^>]*)>/i, `<body$1>\n  ${loaderDiv}`);
      }
    } else {
      if (content.includes(loaderDiv)) {
        updatedContent = content.replace(loaderDiv, '');
      }
    }
    host.overwrite(indexPath, updatedContent);
  };
}
