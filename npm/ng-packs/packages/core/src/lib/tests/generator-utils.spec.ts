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
      name         | charSet     | passedPasswordLength | actualPasswordLength
      ${'lower'}   | ${lowers}   | ${Infinity}          | ${128}
      ${'lower'}   | ${lowers}   | ${129}               | ${128}
      ${'lower'}   | ${lowers}   | ${10}                | ${10}
      ${'lower'}   | ${lowers}   | ${7}                 | ${7}
      ${'upper'}   | ${uppers}   | ${6}                 | ${6}
      ${'number'}  | ${numbers}  | ${5}                 | ${5}
      ${'special'} | ${specials} | ${4}                 | ${4}
      ${'special'} | ${specials} | ${2}                 | ${4}
      ${'special'} | ${specials} | ${0}                 | ${4}
      ${'special'} | ${specials} | ${undefined}         | ${8}
    `(
      'should have a $name in the password that length is $passwordLength',
      ({ _, charSet, passedPasswordLength, actualPasswordLength }) => {
        const password = generatePassword(passedPasswordLength);
        expect(password).toHaveLength(actualPasswordLength);
        expect(hasChar(charSet, password)).toBe(true);
      },
    );
  });
});

function hasChar(charSet: string, password: string): boolean {
  return charSet.split('').some(char => password.indexOf(char) > -1);
}
