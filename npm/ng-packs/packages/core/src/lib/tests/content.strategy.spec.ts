import {
  CONTENT_SECURITY_STRATEGY,
  CONTENT_STRATEGY,
  DOM_STRATEGY,
  ScriptContentStrategy,
  StyleContentStrategy,
} from '../strategies';
import { uuid } from '../utils';

describe('StyleContentStrategy', () => {
  describe('#createElement', () => {
    it('should create a style element', () => {
      const strategy = new StyleContentStrategy('');
      const element = strategy.createElement();

      expect(element.tagName).toBe('STYLE');
    });
  });

  describe('#insertElement', () => {
    it('should use given dom and content security strategies', () => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const contentSecurityStrategy = CONTENT_SECURITY_STRATEGY.None();

      contentSecurityStrategy.applyCSP = jest.fn((el: HTMLScriptElement) => {});
      domStrategy.insertElement = jest.fn((el: HTMLScriptElement) => {}) as any;

      const strategy = new StyleContentStrategy('', domStrategy, contentSecurityStrategy);
      strategy.createElement();
      const element = strategy.insertElement();

      expect(contentSecurityStrategy.applyCSP).toHaveBeenCalledWith(element);
      expect(domStrategy.insertElement).toHaveBeenCalledWith(element);
    });
  });
});

describe('ScriptContentStrategy', () => {
  describe('#createElement', () => {
    it('should create a style element', () => {
      const nonce = uuid();
      const strategy = new ScriptContentStrategy('');
      const element = strategy.createElement();

      expect(element.tagName).toBe('SCRIPT');
    });
  });

  describe('#insertElement', () => {
    it('should use given dom and content security strategies', () => {
      const nonce = uuid();

      const domStrategy = DOM_STRATEGY.PrependToHead();
      const contentSecurityStrategy = CONTENT_SECURITY_STRATEGY.Loose(nonce);

      contentSecurityStrategy.applyCSP = jest.fn((el: HTMLScriptElement) => {});
      domStrategy.insertElement = jest.fn((el: HTMLScriptElement) => {}) as any;

      const strategy = new ScriptContentStrategy('', domStrategy, contentSecurityStrategy);
      const element = strategy.createElement();
      strategy.insertElement();

      expect(contentSecurityStrategy.applyCSP).toHaveBeenCalledWith(element);
      expect(domStrategy.insertElement).toHaveBeenCalledWith(element);
    });
  });
});

describe('CONTENT_STRATEGY', () => {
  test.each`
    name                    | Strategy                 | domStrategy
    ${'AppendScriptToBody'} | ${ScriptContentStrategy} | ${'AppendToBody'}
    ${'AppendScriptToHead'} | ${ScriptContentStrategy} | ${'AppendToHead'}
    ${'AppendStyleToHead'}  | ${StyleContentStrategy}  | ${'AppendToHead'}
    ${'PrependStyleToHead'} | ${StyleContentStrategy}  | ${'PrependToHead'}
  `(
    'should successfully map $name to $Strategy.name with $domStrategy dom strategy',
    ({ name, Strategy, domStrategy }) => {
      expect(CONTENT_STRATEGY[name]('')).toEqual(new Strategy('', DOM_STRATEGY[domStrategy]()));
    },
  );
});
