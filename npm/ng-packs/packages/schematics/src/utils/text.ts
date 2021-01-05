import { strings } from '@angular-devkit/core';

export const lower = (text: string) => text.toLowerCase();
export const upper = (text: string) => text.toUpperCase();
export const camel = (text: string) => toCamelCase(_(text));
export const pascal = (text: string) => strings.classify(_(text));
export const kebab = (text: string) => strings.dasherize(_(text));
export const snake = (text: string) => strings.underscore(_(text));
export const macro = (text: string) => upper(snake(text));
export const dir = (text: string) =>
  strings.dasherize(text.replace(/\./g, '/').replace(/\/\//g, '/'));

export const quote = (value: number | string) =>
  typeof value === 'string' ? `'${value.replace(/'/g, '\\\'')}'` : value;

function _(text: string): string {
  return text.replace(/\./g, '_');
}

// https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L155
function toCamelCase(str: string) {
  if (!str || !isUpperCase(str[0])) return str;

  const chars = str.split('');
  const { length } = chars;

  for (let i = 0; i < length; i++) {
    if (i === 1 && !isUpperCase(chars[i])) break;

    const hasNext = i + 1 < length;

    if (i > 0 && hasNext && !isUpperCase(chars[i + 1])) {
      if (isSeparator(chars[i + 1])) {
        chars[i] = toLowerCase(chars[i]);
      }

      break;
    }

    chars[i] = toLowerCase(chars[i]);
  }

  return chars.join('');
}

function isSeparator(str = '') {
  return /[\s\u2000-\u206F\u2E00-\u2E7F\\'!"#$%&()*+,\-.\/:;<=>?@\[\]^_`{|}~]+/.test(str);
}

function isUpperCase(str = '') {
  return /[A-Z]+/.test(str);
}

function toLowerCase(str = '') {
  return str.toLowerCase();
}
