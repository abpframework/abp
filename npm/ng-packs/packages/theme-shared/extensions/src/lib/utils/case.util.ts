// https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L155
export function jsonNetCamelCase(str: string) {
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
