import { pushValueTo } from '../utils/array-utils';

describe('Array Utils', () => {
  describe('#pushValueTo', () => {
    test.each`
      source          | target          | expected
      ${[]}           | ${[0, 1, 2, 3]} | ${[0, 1, 2, 3]}
      ${[3]}          | ${[0, 1, 2]}    | ${[0, 1, 2, 3]}
      ${[2, 3]}       | ${[0, 1]}       | ${[0, 1, 2, 3]}
      ${[1, 2, 3]}    | ${[0]}          | ${[0, 1, 2, 3]}
      ${[0, 1, 2, 3]} | ${[]}           | ${[0, 1, 2, 3]}
    `('should push $source to $target when called in forEach', ({ source, target, expected }) => {
      source.forEach(pushValueTo(target));
      expect(target).toEqual(expected);
    });
  });
});
