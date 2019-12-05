export interface GenerateArgsParam {
  key: string;
  type: string;
  isOptional?: boolean;
}

export function generateArgs(args: GenerateArgsParam[]): string {
  return args.reduce((acc, val) => {
    const arg = `${val.key}${val.isOptional ? '?' : ''}: ${val.type}`;
    if (acc) return `${acc}, ${arg}`;
    else return arg;
  }, '');
}
