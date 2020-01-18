import { noop } from '../utils';

describe('CommonUtils', () => {
  describe('#noop', () => {
    test('should return empty fn', () => {
      expect(typeof noop()).toBe('function');
      expect(noop()()).toBeUndefined();
    });
  });
});
