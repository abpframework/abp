import { UrlSegment } from '@angular/router';
import {
  createRouteCultureUrlMatcher,
  isLikelyCultureSegment,
} from '../utils/route-culture-url-matcher';

describe('route-culture-url-matcher', () => {
  describe('#isLikelyCultureSegment', () => {
    test.each`
      segment        | expected
      ${''}          | ${false}
      ${'en'}        | ${true}
      ${'tr'}        | ${true}
      ${'zh-Hans'}   | ${true}
      ${'pt-BR'}     | ${true}
      ${'home'}      | ${false}
      ${'identity'}  | ${false}
      ${'cms-kit'}   | ${false}
      ${'account'}   | ${false}
    `('should return $expected when segment is $segment', ({ segment, expected }) => {
      expect(isLikelyCultureSegment(segment)).toBe(expected);
    });
  });

  describe('#createRouteCultureUrlMatcher', () => {
    const matcher = createRouteCultureUrlMatcher();

    test('should consume first segment when it is a culture code', () => {
      const segments = [new UrlSegment('en', {}), new UrlSegment('home', {})];
      const result = matcher(segments, null as any, {} as any);
      expect(result?.consumed).toEqual([segments[0]]);
      expect(result?.posParams?.culture).toBe(segments[0]);
    });

    test('should return null when first segment is not a culture code', () => {
      const segments = [new UrlSegment('identity', {})];
      expect(matcher(segments, null as any, {} as any)).toBeNull();
    });

    test('should return null when there are no segments', () => {
      expect(matcher([], null as any, {} as any)).toBeNull();
    });
  });
});
