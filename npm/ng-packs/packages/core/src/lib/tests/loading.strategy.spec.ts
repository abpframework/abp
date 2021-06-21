import {
  CROSS_ORIGIN_STRATEGY,
  DOM_STRATEGY,
  LOADING_STRATEGY,
  ScriptLoadingStrategy,
  StyleLoadingStrategy,
} from '../strategies';

const path = 'http://example.com/';

describe('ScriptLoadingStrategy', () => {
  describe('#createElement', () => {
    it('should return a script element with src attribute', () => {
      const strategy = new ScriptLoadingStrategy(path);
      const element = strategy.createElement();

      expect(element.tagName).toBe('SCRIPT');
      expect(element.src).toBe(path);
    });
  });

  describe('#createStream', () => {
    it('should use given dom and cross-origin strategies', done => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const crossOriginStrategy = CROSS_ORIGIN_STRATEGY.UseCredentials();

      domStrategy.insertElement = jest.fn((el: HTMLScriptElement) => {
        setTimeout(() => {
          el.onload(
            new CustomEvent('success', {
              detail: {
                crossOrigin: el.crossOrigin,
              },
            }),
          );
        }, 0);
      }) as any;

      const strategy = new ScriptLoadingStrategy(path, domStrategy, crossOriginStrategy);

      strategy.createStream<CustomEvent>().subscribe(event => {
        expect(strategy.element.tagName).toBe('SCRIPT');
        expect(event.detail.crossOrigin).toBe('use-credentials');
        done();
      });
    });
  });
});

describe('StyleLoadingStrategy', () => {
  describe('#createElement', () => {
    it('should return a style element with href and rel attributes', () => {
      const strategy = new StyleLoadingStrategy(path);
      const element = strategy.createElement();

      expect(element.tagName).toBe('LINK');
      expect(element.href).toBe(path);
      expect(element.rel).toBe('stylesheet');
    });
  });

  describe('#createStream', () => {
    it('should use given dom and cross-origin strategies', done => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const crossOriginStrategy = CROSS_ORIGIN_STRATEGY.UseCredentials();

      domStrategy.insertElement = jest.fn((el: HTMLLinkElement) => {
        setTimeout(() => {
          el.onload(
            new CustomEvent('success', {
              detail: {
                crossOrigin: el.crossOrigin,
              },
            }),
          );
        }, 0);
      }) as any;

      const strategy = new StyleLoadingStrategy(path, domStrategy, crossOriginStrategy);

      strategy.createStream<CustomEvent>().subscribe(event => {
        expect(strategy.element.tagName).toBe('LINK');
        expect(event.detail.crossOrigin).toBe('use-credentials');
        done();
      });
    });
  });
});

describe('LOADING_STRATEGY', () => {
  test.each`
    name                              | Strategy                 | domStrategy
    ${'AppendAnonymousScriptToBody'}  | ${ScriptLoadingStrategy} | ${'AppendToBody'}
    ${'AppendAnonymousScriptToHead'}  | ${ScriptLoadingStrategy} | ${'AppendToHead'}
    ${'AppendAnonymousStyleToHead'}   | ${StyleLoadingStrategy}  | ${'AppendToHead'}
    ${'PrependAnonymousScriptToHead'} | ${ScriptLoadingStrategy} | ${'PrependToHead'}
    ${'PrependAnonymousStyleToHead'}  | ${StyleLoadingStrategy}  | ${'PrependToHead'}
  `(
    'should successfully map $name to $Strategy.name with $domStrategy dom strategy',
    ({ name, Strategy, domStrategy }) => {
      expect(LOADING_STRATEGY[name](path)).toEqual(new Strategy(path, DOM_STRATEGY[domStrategy]()));
    },
  );
});
