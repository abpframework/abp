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
      const obj = {
        a: { b: { c: { x: 1036 } } },
      };

      expect(service.byDeep<typeof obj>('a', 'b', 'c', 'x')(284, obj)).toBe(1036);
    });
  });
});
