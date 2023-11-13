import { ActionData } from '../lib/models/actions';

describe('ActionData', () => {
  describe('#data', () => {
    it('should return record and getInjected', () => {
      const spy = jest.fn();
      class Data extends ActionData<string> {
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
