import { SchematicsException, Tree } from '@angular-devkit/schematics';
import * as ts from 'typescript';
import { Exception } from '../enums';
import { Controller, Type } from '../models';

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

export function removeDefaultPlaceholders<T extends { [key: string]: any }>(oldParams: T) {
  const newParams: Record<string, string | undefined> = {};

  Object.entries(oldParams).forEach(([key, value]) => {
    newParams[key] = value === '__default' ? undefined : value;
  });

  return newParams as T;
}

const sanitizeTypeNameRegExp = /\+/g;
const sanitizeTypeName = (name: string) => name.replace(sanitizeTypeNameRegExp, '_');

export function sanitizeControllerTypeNames(
  controllers: Record<string, Controller>,
): Record<string, Controller> {
  Object.values(controllers || {}).forEach(controller => {
    controller.interfaces?.forEach(i => {
      i.methods?.forEach(m => {
        m.returnValue.type = sanitizeTypeName(m.returnValue.type);
        m.returnValue.typeSimple = sanitizeTypeName(m.returnValue.typeSimple);
        m.parametersOnMethod?.forEach(p => {
          p.type = sanitizeTypeName(p.type);
          p.typeAsString = sanitizeTypeName(p.typeAsString);
          p.typeSimple = sanitizeTypeName(p.typeSimple);
        });
      });
    });

    Object.values(controller.actions || {}).forEach(a => {
      a.returnValue.type = sanitizeTypeName(a.returnValue.type);
      a.returnValue.typeSimple = sanitizeTypeName(a.returnValue.typeSimple);
      a.parametersOnMethod?.forEach(p => {
        p.type = sanitizeTypeName(p.type);
        p.typeAsString = sanitizeTypeName(p.typeAsString);
        p.typeSimple = sanitizeTypeName(p.typeSimple);
      });
      a.parameters?.forEach(p => {
        p.type = sanitizeTypeName(p.type);
        p.typeSimple = sanitizeTypeName(p.typeSimple);
      });
    });
  });

  return controllers;
}

export function sanitizeTypeNames(types: Record<string, Type>): Record<string, Type> {
  // sanitize typeNames, type, and typeSimple on properties
  return Object.entries(types).reduce((acc, [key, value]) => {
    const sanitized = sanitizeTypeName(key);
    const properties =
      value.properties?.map(p => {
        const t = sanitizeTypeName(p.type);
        const t2 = sanitizeTypeName(p.typeSimple);
        return { ...p, type: t, typeSimple: t2 };
      }) || null;
    return { ...acc, [sanitized]: { ...value, properties } };
  }, {});
}
