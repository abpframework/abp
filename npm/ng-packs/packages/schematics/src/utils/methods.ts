import { camel } from './text';

const shouldQuote = require('should-quote');
export const getParamName = (paramName: string) =>
  shouldQuote(paramName) ? `["${paramName}"]` : paramName;

export const getParamValueName = (paramName: string, descriptorName: string) => {
  if (paramName.includes('.')) {
    const splitted = paramName.split('.');
    const param = splitted.map(x => (shouldQuote(x) ? `[${x}]` : `.${camel(x)}`)).join('');
    return `${descriptorName}${param}`;
  }
  if (shouldQuote(paramName)) {
    return `${descriptorName}['${paramName}']`;
  }
  return `${descriptorName}.${paramName}`;
};

export function isDictionaryType(type?: string, typeSimple?: string): boolean {
  const haystacks = [type || '', typeSimple || ''];
  return haystacks.some(t => /(^|\b)(System\.Collections\.Generic\.)?(I)?Dictionary\s*</.test(t));
}

export function isCollectionType(type?: string, typeSimple?: string): boolean {
  const haystacks = [type || '', typeSimple || ''];
  return haystacks.some(t => /(^|\b)(System\.Collections\.Generic\.)?(I)?(List|Enumerable|Collection)\s*</.test(t));
}