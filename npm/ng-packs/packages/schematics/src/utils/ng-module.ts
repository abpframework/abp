import { SchematicsException, Tree, UpdateRecorder } from '@angular-devkit/schematics';
import { getMainFilePath } from './angular/standalone/util';
import * as ts from 'typescript';
import { getAppModulePath, getDecoratorMetadata, getMetadataField } from './angular';
import { createSourceFile } from '../commands/change-theme/index';
import { normalize, Path } from '@angular-devkit/core';
import * as path from 'path';
import { removeEmptyElementsFromArrayLiteral } from './ast';

/**
 * Checks whether a specific import or provider exists in the specified metadata
 * array (`imports`, `providers`, etc.) of the `NgModule` decorator in the AppModule.
 *
 * This function locates the AppModule file of the given Angular project,
 * parses its AST, and inspects the specified metadata array to determine
 * if it includes an element matching the provided string (e.g., `CommonModule`, `HttpClientModule`).
 *
 * @param host - The virtual file system tree used by Angular schematics.
 * @param projectName - The name of the Angular project.
 * @param metadataFn - The name (string) to match against the elements of the metadata array.
 * @param metadataName - The metadata field to search in (e.g., 'imports', 'providers'). Defaults to 'imports'.
 * @returns A promise that resolves to `true` if the metadata function is found, or `false` otherwise.
 * @throws SchematicsException if the AppModule file or expected metadata is not found or malformed.
 */
export const hasImportInNgModule = async (
  host: Tree,
  projectName: string,
  metadataFn: string,
  metadataName = 'imports',
): Promise<boolean> => {
  const mainFilePath = await getMainFilePath(host, projectName);
  const appModulePath = getAppModulePath(host, mainFilePath);
  const buffer = host.read(appModulePath);

  if (!buffer) {
    throw new SchematicsException(`Could not read file: ${appModulePath}`);
  }

  const source = createSourceFile(host, appModulePath);

  // Get the NgModule decorator metadata
  const ngModuleDecorator = getDecoratorMetadata(source, 'NgModule', '@angular/core')[0];

  if (!ngModuleDecorator) {
    throw new SchematicsException('The app module does not found');
  }

  const matchingProperties = getMetadataField(
    ngModuleDecorator as ts.ObjectLiteralExpression,
    metadataName,
  );
  const assignment = matchingProperties[0] as ts.PropertyAssignment;
  const assignmentInit = assignment.initializer as ts.ArrayLiteralExpression;

  const elements = assignmentInit.elements;
  if (!elements || elements.length < 1) {
    throw new SchematicsException(`Elements could not found: ${elements}`);
  }

  return elements.some(f => f.getText().match(metadataFn));
};

/**
 * Attempts to locate the path of the `AppRoutingModule` file that is imported
 * within the root AppModule file of an Angular application.
 *
 * This function reads the AppModule file (resolved from the main file path),
 * parses its AST, and searches for an import declaration that imports
 * `AppRoutingModule`. Once found, it resolves the import path to a normalized
 * file path relative to the workspace root.
 *
 * @param tree - The virtual file system tree used by Angular schematics.
 * @param mainFilePath - The path to the main entry file of the Angular application (typically `main.ts`).
 * @returns A normalized workspace-relative path to the AppRoutingModule file if found, or `null` otherwise.
 * @throws If the route file path is resolved but the file does not exist in the tree.
 */
export async function findAppRoutesModulePath(
  tree: Tree,
  mainFilePath: string,
): Promise<Path | null> {
  const appModulePath = getAppModulePath(tree, mainFilePath);
  if (!appModulePath || !tree.exists(appModulePath)) return null;

  const buffer = tree.read(appModulePath);
  if (!buffer) return null;

  const source = ts.createSourceFile(
    appModulePath,
    buffer.toString('utf-8'),
    ts.ScriptTarget.Latest,
    true,
  );

  for (const stmt of source.statements) {
    if (!ts.isImportDeclaration(stmt)) continue;

    const importClause = stmt.importClause;
    if (!importClause?.namedBindings || !ts.isNamedImports(importClause.namedBindings)) continue;

    const isRoutesImport = importClause.namedBindings.elements.some(
      el => el.name.getText() === 'AppRoutingModule',
    );
    if (!isRoutesImport || !ts.isStringLiteral(stmt.moduleSpecifier)) continue;

    let importPath = stmt.moduleSpecifier.text;

    if (!importPath.endsWith('.ts')) {
      importPath += '.ts';
    }

    const configDir = path.dirname(appModulePath);
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
 * Cleans up empty or invalid expressions (e.g., extra commas) from the `imports` and `providers`
 * arrays in the NgModule decorator of an Angular module file.
 *
 * This function parses the source file's AST, locates the `NgModule` decorator, and processes
 * the `imports` and `providers` metadata fields. If these fields contain array literals with
 * empty slots (such as trailing or double commas), they are removed and the array is rewritten.
 *
 * @param source - The TypeScript source file containing the Angular module.
 * @param recorder - The recorder used to apply changes to the source file.
 */
export function cleanEmptyExprFromModule(source: ts.SourceFile, recorder: UpdateRecorder): void {
  const ngModuleNode = getDecoratorMetadata(source, 'NgModule', '@angular/core')[0];
  if (!ngModuleNode) return;

  const printer = ts.createPrinter();
  const metadataKeys = ['imports', 'providers'];
  for (const key of metadataKeys) {
    const metadataField = getMetadataField(ngModuleNode as ts.ObjectLiteralExpression, key);
    if (!metadataField.length) continue;

    const assignment = metadataField[0] as ts.PropertyAssignment;
    const arrayLiteral = assignment.initializer as ts.ArrayLiteralExpression;

    const cleanedArray = removeEmptyElementsFromArrayLiteral(arrayLiteral);

    recorder.remove(arrayLiteral.getStart(), arrayLiteral.getWidth());
    recorder.insertLeft(
      arrayLiteral.getStart(),
      printer.printNode(ts.EmitHint.Expression, cleanedArray, source),
    );
  }
}
