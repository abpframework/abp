import { APIDefination } from '../types/api-defination';

export interface Argument {
  key: string;
  type: string;
  isOptional?: boolean;
}

export function generateArgs(args: Argument[]): string {
  return args.reduce((acc, val) => {
    const arg = `${val.key}${val.isOptional ? '?' : ''}: ${val.type}`;
    if (acc) return `${acc}, ${arg}`;
    else return arg;
  }, '');
}

export function parseParameters(parameters: APIDefination.Parameter[]): Argument[] {
  return parameters
    .filter(param => param.bindingSourceId === 'Path')
    .map(param => {
      return { key: param.name, type: findType(param.typeAsString), isOptional: param.isOptional };
    });
}

export function findType(typeAsString: string): string {
  if (typeAsString.indexOf('Guid') > -1) return 'string';
  return 'any';
}
