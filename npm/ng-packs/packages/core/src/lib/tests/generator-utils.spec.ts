import { generateHash } from '../utils';

describe('GeneratorUtils', () => {
  describe('#generateHash', () => {
    test('should generate a hash', async () => {
      const hash = generateHash('some content \n with second line');
      expect(hash).toBe(1112440527);
    });
  });
});
