export function isNumber(value: string | number): boolean {
  /* tslint:disable-next-line:triple-equals */
  return value == Number(value);
}
