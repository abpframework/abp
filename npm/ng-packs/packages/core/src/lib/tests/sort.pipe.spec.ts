import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { SortPipe } from '../pipes/sort.pipe';

describe('SortPipe', () => {
  let pipe: SortPipe;
  let spectator: SpectatorService<SortPipe>;
  const createService = createServiceFactory(SortPipe);

  beforeEach(() => {
    spectator = createService();
    pipe = spectator.service;
  });

  test('should sort array in ascending and descending orders', () => {
    expect(pipe.transform([5, 'b', 1, 'a'], 'asc')).toEqual([1, 5, 'a', 'b']);
    expect(pipe.transform([5, 'b', 1, 'a'], 'desc')).toEqual(['b', 'a', 5, 1]);
  });

  test('should sort object array in given order with given key', () => {
    const array = [{ key: 5 }, { key: 'b' }, { key: 1 }, { key: 'a' }, { key: null }];

    expect(pipe.transform(array, 'asc', 'key')).toEqual([
      { key: 1 },
      { key: 5 },
      { key: 'a' },
      { key: 'b' },
      { key: null },
    ]);
    expect(pipe.transform(array, 'desc', 'key')).toEqual([
      { key: null },
      { key: 'b' },
      { key: 'a' },
      { key: 5 },
      { key: 1 },
    ]);
  });

  test('should require an array as value', () => {
    expect(pipe.transform(null)).toBeFalsy();
    expect(pipe.transform(undefined, 'desc')).toBeFalsy();
  });
});
