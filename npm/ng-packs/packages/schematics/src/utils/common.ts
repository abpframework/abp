import { SchematicsException, Tree } from '@angular-devkit/schematics';
import * as ts from 'typescript';
import { Exception } from '../enums';

export function interpolate(text: string, ...params: (string | number | boolean)[]) {
  params.forEach((param, i) => {
    const pattern = new RegExp('{\\s*' + i + '\\s*}');
    text = text.replace(pattern, String(param));
  });

  return text;
}

export function isNullOrUndefined(value: any): value is null | undefined {
  return value === null || value === undefined;
}

export function readFileInTree(tree: Tree, filePath: string): ts.SourceFile {
  const buffer = tree.read(filePath);
  if (isNullOrUndefined(buffer))
    throw new SchematicsException(interpolate(Exception.FileNotFound, filePath));

  const text = buffer.toString('utf-8');
  return ts.createSourceFile(filePath, text, ts.ScriptTarget.Latest, true);
}
