// eslint-disable-next-line @typescript-eslint/no-var-requires
const shouldQuote = require('should-quote');
export const getParamName = (paramName: string) =>
  shouldQuote(paramName) ? `["${paramName}"]` : paramName;
