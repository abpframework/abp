export function createTokenParser(format: string) {
  return (string: string) => {
    const tokens: string[] = [];
    const regex = format.replace(/\./g, '\\.').replace(/\{\s?([0-9a-zA-Z]+)\s?\}/g, (_, token) => {
      tokens.push(token);
      return '(.+)';
    });

    const matches = (string.match(regex) || []).slice(1);

    return matches.reduce((acc, v, i) => {
      const key = tokens[i];
      acc[key] = [...(acc[key] || []), v].filter(Boolean);
      return acc;
    }, {} as Record<string, string[]>);
  };
}
