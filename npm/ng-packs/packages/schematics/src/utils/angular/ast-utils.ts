/**
 * @license
 * Copyright Google Inc. All Rights Reserved.
 *
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://angular.io/license
 */
import * as ts from 'typescript';
import { Change, InsertChange, NoopChange } from './change';

/**
 * Add Import `import { symbolName } from fileName` if the import doesn't exit
 * already. Assumes fileToEdit can be resolved and accessed.
 * @param fileToEdit (file we want to add import to)
 * @param symbolName (item to import)
 * @param fileName (path to the file)
 * @param isDefault (if true, import follows style for importing default exports)
 * @return Change
 */
export function insertImport(
  source: ts.SourceFile,
  fileToEdit: string,
  symbolName: string,
  fileName: string,
  isDefault = false,
): Change {
  const rootNode = source;
  const allImports = findNodes(rootNode, ts.SyntaxKind.ImportDeclaration);

  // get nodes that map to import statements from the file fileName
  const relevantImports = allImports.filter(node => {
    // StringLiteral of the ImportDeclaration is the import file (fileName in this case).
    const importFiles = node
      .getChildren()
      .filter(ts.isStringLiteral)
      .map(n => n.text);

    return importFiles.filter(file => file === fileName).length === 1;
  });

  if (relevantImports.length > 0) {
    let importsAsterisk = false;
    // imports from import file
    const imports: ts.Node[] = [];
    relevantImports.forEach(n => {
      Array.prototype.push.apply(imports, findNodes(n, ts.SyntaxKind.Identifier));
      if (findNodes(n, ts.SyntaxKind.AsteriskToken).length > 0) {
        importsAsterisk = true;
      }
    });

    // if imports * from fileName, don't add symbolName
    if (importsAsterisk) {
      return new NoopChange();
    }

    const importTextNodes = imports.filter(n => (n as ts.Identifier).text === symbolName);

    // insert import if it's not there
    if (importTextNodes.length === 0) {
      const fallbackPos =
        findNodes(relevantImports[0], ts.SyntaxKind.CloseBraceToken)[0].getStart() ||
        findNodes(relevantImports[0], ts.SyntaxKind.FromKeyword)[0].getStart();

      return insertAfterLastOccurrence(imports, `, ${symbolName}`, fileToEdit, fallbackPos);
    }

    return new NoopChange();
  }

  // no such import declaration exists
  const useStrict = findNodes(rootNode, ts.isStringLiteral).filter(n => n.text === 'use strict');
  let fallbackPos = 0;
  if (useStrict.length > 0) {
    fallbackPos = useStrict[0].end;
  }
  const open = isDefault ? '' : '{ ';
  const close = isDefault ? '' : ' }';
  // if there are no imports or 'use strict' statement, insert import at beginning of file
  const insertAtBeginning = allImports.length === 0 && useStrict.length === 0;
  const separator = insertAtBeginning ? '' : ';\n';
  const toInsert =
    `${separator}import ${open}${symbolName}${close}` +
    ` from '${fileName}'${insertAtBeginning ? ';\n' : ''}`;

  return insertAfterLastOccurrence(
    allImports,
    toInsert,
    fileToEdit,
    fallbackPos,
    ts.SyntaxKind.StringLiteral,
  );
}

/**
 * Find all nodes from the AST in the subtree of node of SyntaxKind kind.
 * @param node
 * @param kind
 * @param max The maximum number of items to return.
 * @param recursive Continue looking for nodes of kind recursive until end
 * the last child even when node of kind has been found.
 * @return all nodes of kind, or [] if none is found
 */
export function findNodes(
  node: ts.Node,
  kind: ts.SyntaxKind,
  max?: number,
  recursive?: boolean,
): ts.Node[];

/**
 * Find all nodes from the AST in the subtree that satisfy a type guard.
 * @param node
 * @param guard
 * @param max The maximum number of items to return.
 * @param recursive Continue looking for nodes of kind recursive until end
 * the last child even when node of kind has been found.
 * @return all nodes that satisfy the type guard, or [] if none is found
 */
export function findNodes<T extends ts.Node>(
  node: ts.Node,
  guard: (node: ts.Node) => node is T,
  max?: number,
  recursive?: boolean,
): T[];

export function findNodes<T extends ts.Node>(
  node: ts.Node,
  kindOrGuard: ts.SyntaxKind | ((node: ts.Node) => node is T),
  max = Infinity,
  recursive = false,
): T[] {
  if (!node || max == 0) {
    return [];
  }

  const test =
    typeof kindOrGuard === 'function'
      ? kindOrGuard
      : (node: ts.Node): node is T => node.kind === kindOrGuard;

  const arr: T[] = [];
  if (test(node)) {
    arr.push(node);
    max--;
  }
  if (max > 0 && (recursive || !test(node))) {
    for (const child of node.getChildren()) {
      findNodes(child, test, max).forEach(node => {
        if (max > 0) {
          arr.push(node);
        }
        max--;
      });

      if (max <= 0) {
        break;
      }
    }
  }

  return arr;
}

/**
 * Get all the nodes from a source.
 * @param sourceFile The source file object.
 * @returns {Array<ts.Node>} An array of all the nodes in the source.
 */
export function getSourceNodes(sourceFile: ts.SourceFile): ts.Node[] {
  const nodes: ts.Node[] = [sourceFile];
  const result = [];

  while (nodes.length > 0) {
    const node = nodes.shift();

    if (node) {
      result.push(node);
      if (node.getChildCount(sourceFile) >= 0) {
        nodes.unshift(...node.getChildren());
      }
    }
  }

  return result;
}

export function findNode(node: ts.Node, kind: ts.SyntaxKind, text: string): ts.Node | null {
  if (node.kind === kind && node.getText() === text) {
    // throw new Error(node.getText());
    return node;
  }

  let foundNode: ts.Node | null = null;
  ts.forEachChild(node, childNode => {
    foundNode = foundNode || findNode(childNode, kind, text);
  });

  return foundNode;
}

/**
 * Helper for sorting nodes.
 * @return function to sort nodes in increasing order of position in sourceFile
 */
function nodesByPosition(first: ts.Node, second: ts.Node): number {
  return first.getStart() - second.getStart();
}

/**
 * Insert `toInsert` after the last occurence of `ts.SyntaxKind[nodes[i].kind]`
 * or after the last of occurence of `syntaxKind` if the last occurence is a sub child
 * of ts.SyntaxKind[nodes[i].kind] and save the changes in file.
 *
 * @param nodes insert after the last occurence of nodes
 * @param toInsert string to insert
 * @param file file to insert changes into
 * @param fallbackPos position to insert if toInsert happens to be the first occurence
 * @param syntaxKind the ts.SyntaxKind of the subchildren to insert after
 * @return Change instance
 * @throw Error if toInsert is first occurence but fall back is not set
 */
export function insertAfterLastOccurrence(
  nodes: ts.Node[],
  toInsert: string,
  file: string,
  fallbackPos: number,
  syntaxKind?: ts.SyntaxKind,
): Change {
  let lastItem: ts.Node | undefined;
  for (const node of nodes) {
    if (!lastItem || lastItem.getStart() < node.getStart()) {
      lastItem = node;
    }
  }
  if (syntaxKind && lastItem) {
    lastItem = findNodes(lastItem, syntaxKind).sort(nodesByPosition).pop();
  }
  if (!lastItem && fallbackPos == undefined) {
    throw new Error(`tried to insert ${toInsert} as first occurence with no fallback position`);
  }
  const lastItemPosition: number = lastItem ? lastItem.getEnd() : fallbackPos;

  return new InsertChange(file, lastItemPosition, toInsert);
}

export function getContentOfKeyLiteral(_source: ts.SourceFile, node: ts.Node): string | null {
  if (node.kind == ts.SyntaxKind.Identifier) {
    return (node as ts.Identifier).text;
  } else if (node.kind == ts.SyntaxKind.StringLiteral) {
    return (node as ts.StringLiteral).text;
  } else {
    return null;
  }
}

function _angularImportsFromNode(
  node: ts.ImportDeclaration,
  _sourceFile: ts.SourceFile,
): { [name: string]: string } {
  const ms = node.moduleSpecifier;
  let modulePath: string;
  switch (ms.kind) {
    case ts.SyntaxKind.StringLiteral:
      modulePath = (ms as ts.StringLiteral).text;
      break;
    default:
      return {};
  }

  if (!modulePath.startsWith('@angular/')) {
    return {};
  }

  if (node.importClause) {
    if (node.importClause.name) {
      // This is of the form `import Name from 'path'`. Ignore.
      return {};
    } else if (node.importClause.namedBindings) {
      const nb = node.importClause.namedBindings;
      if (nb.kind == ts.SyntaxKind.NamespaceImport) {
        // This is of the form `import * as name from 'path'`. Return `name.`.
        return {
          [(nb as ts.NamespaceImport).name.text + '.']: modulePath,
        };
      } else {
        // This is of the form `import {a,b,c} from 'path'`
        const namedImports = nb as ts.NamedImports;

        return namedImports.elements
          .map((is: ts.ImportSpecifier) => (is.propertyName ? is.propertyName.text : is.name.text))
          .reduce((acc: { [name: string]: string }, curr: string) => {
            acc[curr] = modulePath;

            return acc;
          }, {});
      }
    }

    return {};
  } else {
    // This is of the form `import 'path';`. Nothing to do.
    return {};
  }
}

export function getDecoratorMetadata(
  source: ts.SourceFile,
  identifier: string,
  module: string,
): ts.Node[] {
  const angularImports = findNodes(source, ts.isImportDeclaration)
    .map(node => _angularImportsFromNode(node, source))
    .reduce((acc, current) => {
      for (const key of Object.keys(current)) {
        acc[key] = current[key];
      }

      return acc;
    }, {});

  return getSourceNodes(source)
    .filter(node => {
      return (
        node.kind == ts.SyntaxKind.Decorator &&
        (node as ts.Decorator).expression.kind == ts.SyntaxKind.CallExpression
      );
    })
    .map(node => (node as ts.Decorator).expression as ts.CallExpression)
    .filter(expr => {
      if (expr.expression.kind == ts.SyntaxKind.Identifier) {
        const id = expr.expression as ts.Identifier;

        return id.text == identifier && angularImports[id.text] === module;
      } else if (expr.expression.kind == ts.SyntaxKind.PropertyAccessExpression) {
        // This covers foo.NgModule when importing * as foo.
        const paExpr = expr.expression as ts.PropertyAccessExpression;
        // If the left expression is not an identifier, just give up at that point.
        if (paExpr.expression.kind !== ts.SyntaxKind.Identifier) {
          return false;
        }

        const id = paExpr.name.text;
        const moduleId = (paExpr.expression as ts.Identifier).text;

        return id === identifier && angularImports[moduleId + '.'] === module;
      }

      return false;
    })
    .filter(
      expr => expr.arguments[0] && expr.arguments[0].kind == ts.SyntaxKind.ObjectLiteralExpression,
    )
    .map(expr => expr.arguments[0] as ts.ObjectLiteralExpression);
}

function findClassDeclarationParent(node: ts.Node): ts.ClassDeclaration | undefined {
  if (ts.isClassDeclaration(node)) {
    return node;
  }

  return node.parent && findClassDeclarationParent(node.parent);
}

/**
 * Given a source file with @NgModule class(es), find the name of the first @NgModule class.
 *
 * @param source source file containing one or more @NgModule
 * @returns the name of the first @NgModule, or `undefined` if none is found
 */
export function getFirstNgModuleName(source: ts.SourceFile): string | undefined {
  // First, find the @NgModule decorators.
  const ngModulesMetadata = getDecoratorMetadata(source, 'NgModule', '@angular/core');
  if (ngModulesMetadata.length === 0) {
    return undefined;
  }

  // Then walk parent pointers up the AST, looking for the ClassDeclaration parent of the NgModule
  // metadata.
  const moduleClass = findClassDeclarationParent(ngModulesMetadata[0]);
  if (!moduleClass || !moduleClass.name) {
    return undefined;
  }

  // Get the class name of the module ClassDeclaration.
  return moduleClass.name.text;
}

export function getMetadataField(
  node: ts.ObjectLiteralExpression,
  metadataField: string,
): ts.ObjectLiteralElement[] {
  return (
    node.properties
      .filter(ts.isPropertyAssignment)
      // Filter out every fields that's not "metadataField". Also handles string literals
      // (but not expressions).
      .filter(({ name }) => {
        return (
          (ts.isIdentifier(name) || ts.isStringLiteral(name)) && name.getText() === metadataField
        );
      })
  );
}

export function addSymbolToNgModuleMetadata(
  source: ts.SourceFile,
  ngModulePath: string,
  metadataField: string,
  symbolName: string,
  importPath: string | null = null,
): Change[] {
  const nodes = getDecoratorMetadata(source, 'NgModule', '@angular/core');
  let node: any = nodes[0];

  // Find the decorator declaration.
  if (!node) {
    return [];
  }

  // Get all the children property assignment of object literals.
  const matchingProperties = getMetadataField(node as ts.ObjectLiteralExpression, metadataField);

  // Get the last node of the array literal.
  if (!matchingProperties) {
    return [];
  }
  if (matchingProperties.length == 0) {
    // We haven't found the field in the metadata declaration. Insert a new field.
    const expr = node as ts.ObjectLiteralExpression;
    let position: number;
    let toInsert: string;
    if (expr.properties.length == 0) {
      position = expr.getEnd() - 1;
      toInsert = `  ${metadataField}: [${symbolName}]\n`;
    } else {
      node = expr.properties[expr.properties.length - 1];
      position = node.getEnd();
      // Get the indentation of the last element, if any.
      const text = node.getFullText(source);
      const matches = text.match(/^\r?\n\s*/);
      if (matches && matches.length > 0) {
        toInsert = `,${matches[0]}${metadataField}: [${symbolName}]`;
      } else {
        toInsert = `, ${metadataField}: [${symbolName}]`;
      }
    }
    if (importPath !== null) {
      return [
        new InsertChange(ngModulePath, position, toInsert),
        insertImport(source, ngModulePath, symbolName.replace(/\..*$/, ''), importPath),
      ];
    } else {
      return [new InsertChange(ngModulePath, position, toInsert)];
    }
  }
  const assignment = matchingProperties[0] as ts.PropertyAssignment;

  // If it's not an array, nothing we can do really.
  if (assignment.initializer.kind !== ts.SyntaxKind.ArrayLiteralExpression) {
    return [];
  }

  const arrLiteral = assignment.initializer as ts.ArrayLiteralExpression;
  if (arrLiteral.elements.length == 0) {
    // Forward the property.
    node = arrLiteral;
  } else {
    node = arrLiteral.elements;
  }

  if (!node) {
    console.error('No app module found. Please add your new class to your component.');

    return [];
  }

  if (Array.isArray(node)) {
    // eslint-disable-next-line @typescript-eslint/ban-types
    const nodeArray = node as {} as Array<ts.Node>;
    const symbolsArray = nodeArray.map(node => node.getText());
    if (symbolsArray.includes(symbolName)) {
      return [];
    }

    node = node[node.length - 1];
  }

  let toInsert: string;
  let position = node.getEnd();
  if (node.kind == ts.SyntaxKind.ObjectLiteralExpression) {
    // We haven't found the field in the metadata declaration. Insert a new
    // field.
    const expr = node as ts.ObjectLiteralExpression;
    if (expr.properties.length == 0) {
      position = expr.getEnd() - 1;
      toInsert = `  ${symbolName}\n`;
    } else {
      // Get the indentation of the last element, if any.
      const text = node.getFullText(source);
      if (text.match(/^\r?\r?\n/)) {
        toInsert = `,${text.match(/^\r?\n\s*/)[0]}${symbolName}`;
      } else {
        toInsert = `, ${symbolName}`;
      }
    }
  } else if (node.kind == ts.SyntaxKind.ArrayLiteralExpression) {
    // We found the field but it's empty. Insert it just before the `]`.
    position--;
    toInsert = `${symbolName}`;
  } else {
    // Get the indentation of the last element, if any.
    const text = node.getFullText(source);
    if (text.match(/^\r?\n/)) {
      toInsert = `,${text.match(/^\r?\n(\r?)\s*/)[0]}${symbolName}`;
    } else {
      toInsert = `, ${symbolName}`;
    }
  }
  if (importPath !== null) {
    return [
      new InsertChange(ngModulePath, position, toInsert),
      insertImport(source, ngModulePath, symbolName.replace(/\..*$/, ''), importPath),
    ];
  }

  return [new InsertChange(ngModulePath, position, toInsert)];
}

/**
 * Custom function to insert a declaration (component, pipe, directive)
 * into NgModule declarations. It also imports the component.
 */
export function addDeclarationToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(
    source,
    modulePath,
    'declarations',
    classifiedName,
    importPath,
  );
}

/**
 * Custom function to insert an NgModule into NgModule imports. It also imports the module.
 */
export function addImportToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(source, modulePath, 'imports', classifiedName, importPath);
}

/**
 * Custom function to insert a provider into NgModule. It also imports it.
 */
export function addProviderToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(source, modulePath, 'providers', classifiedName, importPath);
}

/**
 * Custom function to insert an export into NgModule. It also imports it.
 */
export function addExportToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(source, modulePath, 'exports', classifiedName, importPath);
}

/**
 * Custom function to insert an export into NgModule. It also imports it.
 */
export function addBootstrapToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(source, modulePath, 'bootstrap', classifiedName, importPath);
}

/**
 * Custom function to insert an entryComponent into NgModule. It also imports it.
 * @deprecated - Since version 9.0.0 with Ivy, entryComponents is no longer necessary.
 */
export function addEntryComponentToModule(
  source: ts.SourceFile,
  modulePath: string,
  classifiedName: string,
  importPath: string,
): Change[] {
  return addSymbolToNgModuleMetadata(
    source,
    modulePath,
    'entryComponents',
    classifiedName,
    importPath,
  );
}

/**
 * Determine if an import already exists.
 */
export function isImported(
  source: ts.SourceFile,
  classifiedName: string,
  importPath: string,
): boolean {
  const allNodes = getSourceNodes(source);
  const matchingNodes = allNodes
    .filter(ts.isImportDeclaration)
    .filter(
      imp => ts.isStringLiteral(imp.moduleSpecifier) && imp.moduleSpecifier.text === importPath,
    )
    .filter(imp => {
      if (!imp.importClause) {
        return false;
      }
      const nodes = findNodes(imp.importClause, ts.isImportSpecifier).filter(
        n => n.getText() === classifiedName,
      );

      return nodes.length > 0;
    });

  return matchingNodes.length > 0;
}

/**
 * This function returns the name of the environment export
 * whether this export is aliased or not. If the environment file
 * is not imported, then it will return `null`.
 */
export function getEnvironmentExportName(source: ts.SourceFile): string | null {
  // Initial value is `null` as we don't know yet if the user
  // has imported `environment` into the root module or not.
  let environmentExportName: string | null = null;

  const allNodes = getSourceNodes(source);

  allNodes
    .filter(ts.isImportDeclaration)
    .filter(
      declaration =>
        declaration.moduleSpecifier.kind === ts.SyntaxKind.StringLiteral &&
        declaration.importClause !== undefined,
    )
    .map(declaration =>
      // If `importClause` property is defined then the first
      // child will be `NamedImports` object (or `namedBindings`).
      (declaration.importClause as ts.ImportClause).getChildAt(0),
    )
    // Find those `NamedImports` object that contains `environment` keyword
    // in its text. E.g. `{ environment as env }`.
    .filter(ts.isNamedImports)
    .filter(namedImports => namedImports.getText().includes('environment'))
    .forEach(namedImports => {
      for (const specifier of namedImports.elements) {
        // `propertyName` is defined if the specifier
        // has an aliased import.
        const name = specifier.propertyName || specifier.name;

        // Find specifier that contains `environment` keyword in its text.
        // Whether it's `environment` or `environment as env`.
        if (name.text.includes('environment')) {
          environmentExportName = specifier.name.text;
        }
      }
    });

  return environmentExportName;
}

/**
 * Returns the RouterModule declaration from NgModule metadata, if any.
 */
export function getRouterModuleDeclaration(source: ts.SourceFile): ts.Expression | undefined {
  const result = getDecoratorMetadata(source, 'NgModule', '@angular/core') as ts.Node[];
  const node = result[0] as ts.ObjectLiteralExpression;
  const matchingProperties = getMetadataField(node, 'imports');

  if (!matchingProperties) {
    return;
  }

  const assignment = matchingProperties[0] as ts.PropertyAssignment;

  if (assignment.initializer.kind !== ts.SyntaxKind.ArrayLiteralExpression) {
    return;
  }

  const arrLiteral = assignment.initializer as ts.ArrayLiteralExpression;

  return arrLiteral.elements
    .filter(el => el.kind === ts.SyntaxKind.CallExpression)
    .find(el => (el as ts.Identifier).getText().startsWith('RouterModule'));
}

/**
 * Adds a new route declaration to a router module (i.e. has a RouterModule declaration)
 */
export function addRouteDeclarationToModule(
  source: ts.SourceFile,
  fileToAdd: string,
  routeLiteral: string,
): Change {
  const routerModuleExpr = getRouterModuleDeclaration(source);
  if (!routerModuleExpr) {
    throw new Error(`Couldn't find a route declaration in ${fileToAdd}.`);
  }
  const scopeConfigMethodArgs = (routerModuleExpr as ts.CallExpression).arguments;
  if (!scopeConfigMethodArgs.length) {
    const { line } = source.getLineAndCharacterOfPosition(routerModuleExpr.getStart());
    throw new Error(
      `The router module method doesn't have arguments ` + `at line ${line} in ${fileToAdd}`,
    );
  }

  let routesArr: ts.ArrayLiteralExpression | undefined;
  const routesArg = scopeConfigMethodArgs[0];

  // Check if the route declarations array is
  // an inlined argument of RouterModule or a standalone variable
  if (ts.isArrayLiteralExpression(routesArg)) {
    routesArr = routesArg;
  } else {
    const routesVarName = routesArg.getText();
    let routesVar;
    if (routesArg.kind === ts.SyntaxKind.Identifier) {
      routesVar = source.statements.filter(ts.isVariableStatement).find(v => {
        return v.declarationList.declarations[0].name.getText() === routesVarName;
      });
    }

    if (!routesVar) {
      const { line } = source.getLineAndCharacterOfPosition(routesArg.getStart());
      throw new Error(
        `No route declaration array was found that corresponds ` +
          `to router module at line ${line} in ${fileToAdd}`,
      );
    }

    routesArr = findNodes(
      routesVar,
      ts.SyntaxKind.ArrayLiteralExpression,
      1,
    )[0] as ts.ArrayLiteralExpression;
  }

  const occurrencesCount = routesArr.elements.length;
  const text = routesArr.getFullText(source);

  let route: string = routeLiteral;
  let insertPos = routesArr.elements.pos;

  if (occurrencesCount > 0) {
    const lastRouteLiteral = [...routesArr.elements].pop() as ts.Expression;
    const lastRouteIsWildcard =
      ts.isObjectLiteralExpression(lastRouteLiteral) &&
      lastRouteLiteral.properties.some(
        n =>
          ts.isPropertyAssignment(n) &&
          ts.isIdentifier(n.name) &&
          n.name.text === 'path' &&
          ts.isStringLiteral(n.initializer) &&
          n.initializer.text === '**',
      );

    const indentation = text.match(/\r?\n(\r?)\s*/) || [];
    const routeText = `${indentation[0] || ' '}${routeLiteral}`;

    // Add the new route before the wildcard route
    // otherwise we'll always redirect to the wildcard route
    if (lastRouteIsWildcard) {
      insertPos = lastRouteLiteral.pos;
      route = `${routeText},`;
    } else {
      insertPos = lastRouteLiteral.end;
      route = `,${routeText}`;
    }
  }

  return new InsertChange(fileToAdd, insertPos, route);
}
