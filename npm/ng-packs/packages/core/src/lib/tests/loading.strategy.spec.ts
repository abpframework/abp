import { firstValueFrom } from 'rxjs';
import { CROSS_ORIGIN_STRATEGY } from '../strategies/cross-origin.strategy';
import {
  LOADING_STRATEGY,
  ScriptLoadingStrategy,
  StyleLoadingStrategy,
} from '../strategies/loading.strategy';
import { DOM_STRATEGY } from '../strategies/dom.strategy';

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
    it('should use given dom and cross-origin strategies', async () => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const crossOriginStrategy = CROSS_ORIGIN_STRATEGY.UseCredentials();

      domStrategy.insertElement = vi.fn((el: HTMLScriptElement) => {
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

      const event = await firstValueFrom(strategy.createStream<CustomEvent>());
      expect(strategy.element.tagName).toBe('SCRIPT');
      expect(event.detail.crossOrigin).toBe('use-credentials');
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
    it('should use given dom and cross-origin strategies', async () => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const crossOriginStrategy = CROSS_ORIGIN_STRATEGY.UseCredentials();

      domStrategy.insertElement = vi.fn((el: HTMLLinkElement) => {
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

      const event = await firstValueFrom(strategy.createStream<CustomEvent>());
      expect(strategy.element.tagName).toBe('LINK');
      expect(event.detail.crossOrigin).toBe('use-credentials');
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
      const actual = LOADING_STRATEGY[name](path);
      const expected = new Strategy(path, DOM_STRATEGY[domStrategy]());

      // Verify instance type and path
      expect(actual).toBeInstanceOf(Strategy);
      expect(actual.path).toBe(expected.path);

      // Verify element creation produces the same result
      const actualElement = actual.createElement();
      const expectedElement = expected.createElement();
      expect(actualElement.tagName).toBe(expectedElement.tagName);
      expect(actualElement.src || actualElement.href).toBe(
        expectedElement.src || expectedElement.href,
      );
    },
  );
});
