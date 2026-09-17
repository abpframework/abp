import { SchematicsException, Tree, UpdateRecorder } from '@angular-devkit/schematics';
import { findBootstrapApplicationCall, getMainFilePath } from './angular/standalone/util';
import { findAppConfig } from './angular/standalone/app_config';
import * as ts from 'typescript';
import { normalize, Path } from '@angular-devkit/core';
import * as path from 'path';
import { findNodes } from './angular';
import { removeEmptyElementsFromArrayLiteral } from './ast';

/**
 * Retrieves the file path of the application's configuration used in a standalone
 * Angular application setup.
 *
 * This function locates the `bootstrapApplication` call in the main entry file and
 * resolves the path to the configuration object passed to it (typically `appConfig`).
 *
 * @param host - The virtual file system tree used by Angular schematics.
 * @param mainFilePath - The path to the main entry file of the Angular application (e.g., `main.ts`).
 * @returns The resolved file path of the application's configuration, or an empty string if not found.
 */
export const getAppConfigPath = (host: Tree, mainFilePath: string): string => {
  const bootstrapCall = findBootstrapApplicationCall(host, mainFilePath);
  const appConfig = findAppConfig(bootstrapCall, host, mainFilePath);
  return appConfig?.filePath || '';
};

/**
 * Attempts to locate the file path of the `routes` array used in a standalone
 * Angular application configuration.
 *
 * This function resolves the application's config file (typically where `routes` is defined or imported),
 * parses the file, and inspects its import declarations to find the import associated with `routes`.
 * It then resolves and normalizes the file path of the `routes` definition and returns it.
 *
 * @param tree - The virtual file system tree used by Angular schematics.
 * @param mainFilePath - The path to the main entry file of the Angular application (e.g., `main.ts`).
 * @returns The normalized workspace-relative path to the file where `routes` is defined, or `null` if not found.
 * @throws If the `routes` import path is found but the file does not exist in the tree.
 */
export function findAppRoutesPath(tree: Tree, mainFilePath: string): Path | null {
  const appConfigPath = getAppConfigPath(tree, mainFilePath);
  if (!appConfigPath || !tree.exists(appConfigPath)) return null;

  const buffer = tree.read(appConfigPath);
  if (!buffer) return null;

  const source = ts.createSourceFile(
    appConfigPath,
    buffer.toString('utf-8'),
    ts.ScriptTarget.Latest,
    true,
  );

  for (const stmt of source.statements) {
    if (!ts.isImportDeclaration(stmt)) continue;

    const importClause = stmt.importClause;
    if (!importClause?.namedBindings || !ts.isNamedImports(importClause.namedBindings)) continue;

    const isRoutesImport = importClause.namedBindings.elements.some(
      el => el.name.getText() === 'routes',
    );
    if (!isRoutesImport || !ts.isStringLiteral(stmt.moduleSpecifier)) continue;

    let importPath = stmt.moduleSpecifier.text;

    if (!importPath.endsWith('.ts')) {
      importPath += '.ts';
    }

    const configDir = path.dirname(appConfigPath);
    const resolvedFsPath = path.resolve(configDir, importPath);
    const workspaceRelativePath = path.relative(process.cwd(), resolvedFsPath).replace(/\\/g, '/');

    const normalizedPath = normalize(workspaceRelativePath);

    if (!tree.exists(normalizedPath)) {
      throw new Error(`Cannot find routes file: ${normalizedPath}`);
    }

    return normalizedPath;
  }

  return null;
}

/**
 * Checks whether a specific provider is registered in the `providers` array of the
 * standalone application configuration (typically within `app.config.ts`) in an Angular project.
 *
 * This function reads and parses the application configuration file, looks for the
 * `providers` property in the configuration object, and checks whether it includes
 * the specified provider name.
 *
 * @param host - The virtual file system tree used by Angular schematics.
 * @param projectName - The name of the Angular project.
 * @param providerName - The name of the provider to search for (as a string match).
 * @returns A promise that resolves to `true` if the provider is found, otherwise `false`.
 * @throws SchematicsException if the app config file cannot be read.
 */
export const hasProviderInStandaloneAppConfig = async (
  host: Tree,
  projectName: string,
  providerName: string,
): Promise<boolean> => {
  const mainFilePath = await getMainFilePath(host, projectName);
  const appConfigPath = getAppConfigPath(host, mainFilePath);
  const buffer = host.read(appConfigPath);

  if (!buffer) {
    throw new SchematicsException(`Could not read file: ${appConfigPath}`);
  }

  const source = ts.createSourceFile(
    appConfigPath,
    buffer.toString('utf-8'),
    ts.ScriptTarget.Latest,
    true,
  );
  const callExpressions = source.statements
    .flatMap(stmt => (ts.isVariableStatement(stmt) ? stmt.declarationList.declarations : []))
    .flatMap(decl =>
      decl.initializer && ts.isObjectLiteralExpression(decl.initializer)
        ? decl.initializer.properties
        : [],
    )
    .filter(ts.isPropertyAssignment)
    .filter(prop => prop.name.getText() === 'providers');

  if (callExpressions.length === 0) return false;

  const providersArray = callExpressions[0].initializer as ts.ArrayLiteralExpression;
  return providersArray.elements.some(el => el.getText().includes(providerName));
};

/**
 * Cleans up empty or invalid expressions (e.g., extra or trailing commas) from the
 * `providers` array within a standalone Angular application configuration object.
 *
 * This function parses the source file's AST to locate variable declarations that
 * define an object literal. It then searches for a `providers` property and removes
 * any empty elements from its array literal, replacing it with a cleaned version.
 *
 * Typically used in Angular schematics to ensure the `providers` array in `app.config.ts`
 * is free of empty slots after modifications.
 *
 * @param source - The TypeScript source file containing the app configuration.
 * @param recorder - The recorder used to apply changes to the source file.
 */
export function cleanEmptyExprFromProviders(source: ts.SourceFile, recorder: UpdateRecorder): void {
  const varStatements = findNodes(source, ts.isVariableStatement);
  const printer = ts.createPrinter();

  for (const stmt of varStatements) {
    const declList = stmt.declarationList;
    for (const decl of declList.declarations) {
      if (!decl.initializer || !ts.isObjectLiteralExpression(decl.initializer)) continue;

      const obj = decl.initializer;

      const providersProp = obj.properties.find(
        prop =>
          ts.isPropertyAssignment(prop) &&
          ts.isIdentifier(prop.name) &&
          prop.name.text === 'providers',
      ) as ts.PropertyAssignment;

      if (!providersProp || !ts.isArrayLiteralExpression(providersProp.initializer)) continue;

      const arrayLiteral = providersProp.initializer;
      const cleanedArray = removeEmptyElementsFromArrayLiteral(arrayLiteral);

      recorder.remove(arrayLiteral.getStart(), arrayLiteral.getWidth());
      recorder.insertLeft(
        arrayLiteral.getStart(),
        printer.printNode(ts.EmitHint.Expression, cleanedArray, source),
      );
    }
  }
}
