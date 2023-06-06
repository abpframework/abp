import { camel } from './text';

// eslint-disable-next-line @typescript-eslint/no-var-requires
const shouldQuote = require('should-quote');
export const getParamName = (paramName: string) =>
  shouldQuote(paramName) ? `["${paramName}"]` : paramName;

// check dot exists in param name and camelize access continuously
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
