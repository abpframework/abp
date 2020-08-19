import { strings } from '@angular-devkit/core';
import { SYSTEM_TYPES } from '../constants';

export function createTypeSimplifier(solution: string) {
  const optionalRegex = /\?/g;
  const solutionRegex = new RegExp(solution.replace(/\./g, `\.`) + `\.`);
  const voloRegex = /^Volo\.(Abp\.?)(Application\.?)/;

  return (type: string) => {
    type = type.replace(voloRegex, '');
    type = type.replace(solutionRegex, '');
    type = type.replace(optionalRegex, '');
    type = type.replace(
      /System\.([0-9A-Za-z]+)/g,
      (_, match) => SYSTEM_TYPES.get(match) ?? strings.camelize(match),
    );
    type = type.split('.').pop()!;
    type = type.startsWith('[') ? type.slice(1, -1) + '[]' : type;
    return type;
  };
}
