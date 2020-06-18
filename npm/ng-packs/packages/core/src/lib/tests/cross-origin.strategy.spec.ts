import { CrossOriginStrategy, CROSS_ORIGIN_STRATEGY } from '../strategies';
import { uuid } from '../utils';

describe('CrossOriginStrategy', () => {
  describe('#setCrossOrigin', () => {
    it('should set crossorigin attribute', () => {
      const strategy = new CrossOriginStrategy('use-credentials');
      const element = document.createElement('link');
      strategy.setCrossOrigin(element);

      expect(element.crossOrigin).toBe('use-credentials');
    });

    it('should set integrity attribute when given', () => {
      const integrity = uuid();
      const strategy = new CrossOriginStrategy('anonymous', integrity);
      const element = document.createElement('link');
      strategy.setCrossOrigin(element);

      expect(element.crossOrigin).toBe('anonymous');
      expect(element.getAttribute('integrity')).toBe(integrity);
    });
  });
});

describe('CROSS_ORIGIN_STRATEGY', () => {
  test.each`
    name                | integrity    | crossOrigin
    ${'Anonymous'}      | ${undefined} | ${'anonymous'}
    ${'Anonymous'}      | ${uuid()}    | ${'anonymous'}
    ${'UseCredentials'} | ${undefined} | ${'use-credentials'}
    ${'UseCredentials'} | ${uuid()}    | ${'use-credentials'}
  `('should successfully map $name to CrossOriginStrategy', ({ name, integrity, crossOrigin }) => {
    expect(CROSS_ORIGIN_STRATEGY[name](integrity)).toEqual(
      new CrossOriginStrategy(crossOrigin, integrity),
    );
  });
});
