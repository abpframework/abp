import { PropData } from '../lib/models/props';

describe('PropData', () => {
  describe('#data', () => {
    it('should return record and getInjected', () => {
      const spy = jest.fn();
      class Data extends PropData<string> {
        index = 0;
        record = 'X';
        getInjected = spy;
      }

      const data = new Data();
      data.data.getInjected(null);

      expect(spy).toHaveBeenCalledTimes(1);
      expect(spy).toHaveBeenCalledWith(null);
      expect(data.data.index).toBe(0);
      expect(data.data.record).toBe('X');
    });
  });
});
