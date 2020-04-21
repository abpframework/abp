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
      ${'lower'}   | ${lowers}   | ${10}
      ${'lower'}   | ${lowers}   | ${7}
      ${'upper'}   | ${uppers}   | ${6}
      ${'number'}  | ${numbers}  | ${5}
      ${'special'} | ${specials} | ${4}
      ${'special'} | ${specials} | ${2}
      ${'special'} | ${specials} | ${0}
    `(
      'should have a $name in the password that length is $passwordLength',
      ({ _, charSet, passwordLength }) => {
        const password = generatePassword(passwordLength);
        expect(password).toHaveLength(passwordLength < 4 ? 4 : passwordLength);
        expect(hasChar(charSet, password)).toBe(true);
      },
    );
  });
});

function hasChar(charSet: string, password: string): boolean {
  return charSet.split('').some(char => password.indexOf(char) > -1);
}
