import { TrackByService } from '../services/track-by.service';

describe('TrackByService', () => {
  const service = new TrackByService();

  describe('#by', () => {
    it('should return a function which tracks a property', () => {
      expect(service.by('x')(284, { x: 1036 })).toBe(1036);
    });
  });

  describe('#byDeep', () => {
    it('should return a function which tracks a deeply-nested property', () => {
      expect(
        service.byDeep(
          'a',
          'b',
          'c',
          1,
          'x',
        )(284, {
          a: { b: { c: [{ x: 1035 }, { x: 1036 }, { x: 1037 }] } },
        }),
      ).toBe(1036);
    });
  });

  describe('#bySelf', () => {
    it('should return a function which tracks the item', () => {
      expect(service.bySelf()(284, 'X')).toBe('X');
    });
  });
});
