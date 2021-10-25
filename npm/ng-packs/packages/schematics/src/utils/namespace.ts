import { createTypeParser, removeGenerics } from './type';

export function parseNamespace(solution: string, type: string) {
  const parseType = createTypeParser(removeGenerics);
  let namespace = parseType(type)[0].split('.').slice(0, -1).join('.');

  if (solution === namespace) return '';

  solution.split('.').reduceRight((acc, part) => {
    acc = `${part}\\.${acc}`;
    const regex = new RegExp(`^${acc}(Controllers\\.)?`);
    namespace = namespace.replace(regex, '');
    return acc;
  }, '');

  return namespace;
}
