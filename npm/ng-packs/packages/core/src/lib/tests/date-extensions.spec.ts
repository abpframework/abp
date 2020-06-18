import '../utils/date-extensions';

describe('DateExtensions', () => {
  describe('#toLocalISOString', () => {
    test('should able to use as date prototype', () => {
      new Date().toLocalISOString();
    });

    test('should return correct value', () => {
      const now = new Date();
      const timezoneOffset = now.getTimezoneOffset();
      expect(now.toLocalISOString()).toEqual(
        new Date(now.getTime() - timezoneOffset * 60000).toISOString(),
      );
    });
  });
});
