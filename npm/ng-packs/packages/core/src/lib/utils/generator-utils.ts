// tslint:disable: no-bitwise

export function uuid(a?: any): string {
  return a
    ? (a ^ ((Math.random() * 16) >> (a / 4))).toString(16)
    : ('' + 1e7 + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, uuid);
}

export function generateHash(value: string): number {
  let hashed = 0;
  let charCode: number;

  for (let i = 0; i < value.length; i++) {
    charCode = value.charCodeAt(i);
    hashed = (hashed << 5) - hashed + charCode;
    hashed |= 0;
  }
  return hashed;
}
