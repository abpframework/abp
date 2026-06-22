import { Injector, runInInjectionContext } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import {
  CONTENT_SECURITY_STRATEGY,
  CONTENT_STRATEGY,
  DOM_STRATEGY,
  ScriptContentStrategy,
  StyleContentStrategy,
} from '../strategies';
import { uuid } from '../utils';

describe('StyleContentStrategy', () => {
  let injector: Injector;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    injector = TestBed.inject(Injector);
  });

  describe('#createElement', () => {
    it('should create a style element', () => {
      const strategy = new StyleContentStrategy('');
      const element = runInInjectionContext(injector, () => strategy.createElement());

      expect(element.tagName).toBe('STYLE');
    });
  });

  describe('#insertElement', () => {
    it('should use given dom and content security strategies', () => {
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const contentSecurityStrategy = CONTENT_SECURITY_STRATEGY.None();

      contentSecurityStrategy.applyCSP = vi.fn((el: HTMLScriptElement) => {});
      domStrategy.insertElement = vi.fn((el: HTMLScriptElement) => {}) as any;

      const strategy = new StyleContentStrategy('', domStrategy, contentSecurityStrategy);
      runInInjectionContext(injector, () => strategy.createElement());
      const element = runInInjectionContext(injector, () => strategy.insertElement());

      expect(contentSecurityStrategy.applyCSP).toHaveBeenCalledWith(element);
      expect(domStrategy.insertElement).toHaveBeenCalledWith(element);
    });
  });
});

describe('ScriptContentStrategy', () => {
  let injector: Injector;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    injector = TestBed.inject(Injector);
  });

  describe('#createElement', () => {
    it('should create a script element', () => {
      const strategy = new ScriptContentStrategy('');
      const element = runInInjectionContext(injector, () => strategy.createElement());

      expect(element.tagName).toBe('SCRIPT');
    });
  });

  describe('#insertElement', () => {
    it('should use given dom and content security strategies', () => {
      const nonce = uuid();
      const domStrategy = DOM_STRATEGY.PrependToHead();
      const contentSecurityStrategy = CONTENT_SECURITY_STRATEGY.Loose(nonce);

      contentSecurityStrategy.applyCSP = vi.fn((el: HTMLScriptElement) => {});
      domStrategy.insertElement = vi.fn((el: HTMLScriptElement) => {}) as any;

      const strategy = new ScriptContentStrategy('', domStrategy, contentSecurityStrategy);
      const element = runInInjectionContext(injector, () => strategy.createElement());
      runInInjectionContext(injector, () => strategy.insertElement());

      expect(contentSecurityStrategy.applyCSP).toHaveBeenCalledWith(element);
      expect(domStrategy.insertElement).toHaveBeenCalledWith(element);
    });
  });
});

describe('CONTENT_STRATEGY', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  test.each`
    name                    | Strategy                 | domStrategy
    ${'AppendScriptToBody'} | ${ScriptContentStrategy} | ${'AppendToBody'}
    ${'AppendScriptToHead'} | ${ScriptContentStrategy} | ${'AppendToHead'}
    ${'AppendStyleToHead'}  | ${StyleContentStrategy}  | ${'AppendToHead'}
    ${'PrependStyleToHead'} | ${StyleContentStrategy}  | ${'PrependToHead'}
  `(
    'should successfully map $name to $Strategy.name with $domStrategy dom strategy',
    ({ name, Strategy, domStrategy }) => {
      const injector = TestBed.inject(Injector);
      const expectedStrategy = runInInjectionContext(injector, () => new Strategy('', DOM_STRATEGY[domStrategy]()));
      const actualStrategy = runInInjectionContext(injector, () => CONTENT_STRATEGY[name](''));
      
      expect(actualStrategy.constructor).toBe(expectedStrategy.constructor);
      expect(actualStrategy.content).toBe(expectedStrategy.content);
      expect(actualStrategy['domStrategy'].constructor).toBe(expectedStrategy['domStrategy'].constructor);
    },
  );
});
