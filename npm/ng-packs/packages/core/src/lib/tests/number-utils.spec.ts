import { isNumber } from '../utils/number-utils';

describe('Number Utils', () => {
  describe('#isNumber', () => {
    it('should return true if input is a numeric expression', () => {
      expect(isNumber(0)).toBe(true);
      expect(isNumber(0.15)).toBe(true);
      expect(isNumber(2e8)).toBe(true);
      expect(isNumber(Infinity)).toBe(true);

      expect(isNumber('0')).toBe(true);
      expect(isNumber('0.15')).toBe(true);
      expect(isNumber('2e8')).toBe(true);
      expect(isNumber('Infinity')).toBe(true);
    });
  });
});
