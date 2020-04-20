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

export function generatePassword(length = 8) {
  length = length < 4 ? 4 : length;

  const lowers = 'abcdefghijklmnopqrstuvwxyz';
  const uppers = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const numbers = '0123456789';
  const specials = '!@#$%&*()_+{}<>?[]./';
  const all = lowers + uppers + numbers + specials;

  const getRandom = (chrSet: string) => chrSet[Math.floor(Math.random() * chrSet.length)];

  let password = '';

  password += getRandom(lowers);
  password += getRandom(uppers);
  password += getRandom(numbers);
  password += getRandom(specials);

  for (let i = 1; i <= length - 4; i++) {
    password = password.concat(getRandom(all));
  }

  return password
    .split('')
    .sort(() => 0.5 - Math.random())
    .join('');
}
