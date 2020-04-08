import { DomStrategy, DOM_STRATEGY } from '../strategies';
import { CrossOriginStrategy, CROSS_ORIGIN_STRATEGY } from '../strategies/cross-origin.strategy';
import { uuid } from '../utils';
import { fromLazyLoad } from '../utils/lazy-load-utils';

describe('Lazy Load Utils', () => {
  describe('#fromLazyLoad', () => {
    afterEach(() => {
      jest.clearAllMocks();
    });

    it('should append to head by default', () => {
      const element = document.createElement('link');
      const spy = jest.spyOn(document.head, 'insertAdjacentElement');

      fromLazyLoad(element);
      expect(spy).toHaveBeenCalledWith('beforeend', element);
    });

    it('should allow setting a dom strategy', () => {
      const element = document.createElement('link');
      const spy = jest.spyOn(document.head, 'insertAdjacentElement');

      fromLazyLoad(element, DOM_STRATEGY.PrependToHead());
      expect(spy).toHaveBeenCalledWith('afterbegin', element);
    });

    it('should set crossorigin to "anonymous" by default', () => {
      const element = document.createElement('link');

      fromLazyLoad(element);

      expect(element.crossOrigin).toBe('anonymous');
    });

    it('should not set integrity by default', () => {
      const element = document.createElement('link');

      fromLazyLoad(element);

      expect(element.getAttribute('integrity')).toBeNull();
    });

    it('should allow setting a cross-origin strategy', () => {
      const element = document.createElement('link');

      const integrity = uuid();

      fromLazyLoad(element, undefined, CROSS_ORIGIN_STRATEGY.UseCredentials(integrity));

      expect(element.crossOrigin).toBe('use-credentials');
      expect(element.getAttribute('integrity')).toBe(integrity);
    });

    it('should emit error event on fail and clear callbacks', done => {
      const error = new CustomEvent('error');
      const parentNode = { removeChild: jest.fn() };
      const element = ({ parentNode } as any) as HTMLLinkElement;

      fromLazyLoad(
        element,
        {
          insertElement(el: HTMLLinkElement) {
            expect(el).toBe(element);

            setTimeout(() => {
              el.onerror(error);
            }, 0);
          },
        } as DomStrategy,
        {
          setCrossOrigin(_: HTMLLinkElement) {},
        } as CrossOriginStrategy,
      ).subscribe({
        error: value => {
          expect(value).toBe(error);
          expect(parentNode.removeChild).toHaveBeenCalledWith(element);
          expect(element.onerror).toBeNull();
          done();
        },
      });
    });

    it('should emit load event on success and clear callbacks', done => {
      const success = new CustomEvent('load');
      const parentNode = { removeChild: jest.fn() };
      const element = ({ parentNode } as any) as HTMLLinkElement;

      fromLazyLoad(
        element,
        {
          insertElement(el: HTMLLinkElement) {
            expect(el).toBe(element);

            setTimeout(() => {
              el.onload(success);
            }, 0);
          },
        } as DomStrategy,
        {
          setCrossOrigin(_: HTMLLinkElement) {},
        } as CrossOriginStrategy,
      ).subscribe({
        next: value => {
          expect(value).toBe(success);
          expect(parentNode.removeChild).not.toHaveBeenCalled();
          expect(element.onload).toBeNull();
          done();
        },
      });
    });
  });
});
