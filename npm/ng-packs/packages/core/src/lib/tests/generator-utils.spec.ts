import { generateHash, generatePassword } from '../utils';

describe('GeneratorUtils', () => {
  describe('#generateHash', () => {
    test('should generate a hash', async () => {
      const hash = generateHash('some content \n with second line');
      expect(hash).toBe(1112440527);
    });
  });

  describe('#generatePassword', () => {
    const lowers = 'abcdefghijklmnopqrstuvwxyz';
    const uppers = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const numbers = '0123456789';
    const specials = '!@#$%&*()_+{}<>?[]./';

    test.each`
      name         | charSet     | passwordLength
      ${'lower'}   | ${lowers}   | ${7}
      ${'upper'}   | ${uppers}   | ${6}
      ${'number'}  | ${numbers}  | ${5}
      ${'special'} | ${specials} | ${4}
    `(
      'should have a $name in the password that length is $passwordLength',
      ({ _, charSet, passwordLength }) => {
        const password = generatePassword(passwordLength);
        expect(hasChar(charSet, password)).toBe(true);
      },
    );
  });
});

function hasChar(charSet: string, password: string): boolean {
  let matched = false;
  charSet.split('').forEach(char => {
    if (password.indexOf(char) > -1) {
      matched = true;
      return;
    }
  });

  return matched;
}
