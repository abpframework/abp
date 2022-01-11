export function createTokenParser(format: string) {
  return (str: string) => {
    const tokens: string[] = [];
    const regex = format.replace(/\./g, '\\.').replace(/\{\s?([0-9a-zA-Z]+)\s?\}/g, (_, token) => {
      tokens.push(token);
      return '(.+)';
    });

    const matches = (str.match(regex) || []).slice(1);

    return matches.reduce((acc, v, i) => {
      const key = tokens[i];
      acc[key] = [...(acc[key] || []), v].filter(Boolean);
      return acc;
    }, {} as Record<string, string[]>);
  };
}

export function interpolate(text: string, params: string[]) {
  return text
    .replace(/(['"]?\{\s*(\d+)\s*\}['"]?)/g, (_, match, digit) => params[digit] ?? match)
    .replace(/\s+/g, ' ');
}

export function escapeHtmlChars(value: string) {
  return value && value.replace(/</g, '&lt;').replace(/>/g, '&gt;');
}
