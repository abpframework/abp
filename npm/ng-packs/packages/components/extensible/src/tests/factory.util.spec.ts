import { selfFactory } from '../lib/utils/factory.util';

describe('Factory Utils', () => {
  describe('#selfFactory', () => {
    const arr = [];
    const obj = {};
    const date = new Date();
    const promise = Promise.resolve(null);

    test.each`
      parameter
      ${'x'}
      ${''}
      ${1}
      ${0}
      ${true}
      ${false}
      ${arr}
      ${obj}
      ${date}
      ${promise}
      ${null}
      ${undefined}
    `('should return given $parameter back', ({ parameter }) => {
      expect(selfFactory(parameter)).toBe(parameter);
    });
  });
});
