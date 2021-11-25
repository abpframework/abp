import { of, throwError } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { LazyLoadService } from '../services/lazy-load.service';
import { ScriptLoadingStrategy } from '../strategies';
import { ResourceWaitService } from '../services';

describe('LazyLoadService', () => {
  describe('#load', () => {
    const resourceWaitService = new ResourceWaitService();
    const service = new LazyLoadService(resourceWaitService);
    const strategy = new ScriptLoadingStrategy('http://example.com/');

    afterEach(() => {
      jest.clearAllMocks();
    });

    it('should emit an error event if not loaded', done => {
      const counter = jest.fn();
      jest.spyOn(strategy, 'createStream').mockReturnValueOnce(
        of(null).pipe(
          switchMap(() => {
            counter();
            return throwError('THIS WILL NOT BE THE FINAL ERROR');
          }),
        ),
      );

      service.load(strategy, 5, 0).subscribe({
        error: errorEvent => {
          expect(errorEvent).toEqual(new CustomEvent('error'));
          expect(counter).toHaveBeenCalledTimes(6);
          expect(service.loaded.has(strategy.path)).toBe(false);
          done();
        },
      });
    });

    it('should emit a load event if loaded', done => {
      const loadEvent = new CustomEvent('load');
      jest.spyOn(strategy, 'createStream').mockReturnValue(of(loadEvent));

      service.load(strategy).subscribe({
        next: event => {
          expect(event).toBe(loadEvent);
          expect(service.loaded.has(strategy.path)).toBe(true);
          done();
        },
      });
    });

    it('should emit a custom load event if loaded if resource is loaded before', done => {
      const loadEvent = new CustomEvent('load');
      service.loaded.set(strategy.path, null);

      service.load(strategy).subscribe(event => {
        expect(event).toEqual(loadEvent);
        done();
      });
    });
  });

  describe('#remove', () => {
    const resourceWaitService = new ResourceWaitService();
    const service = new LazyLoadService(resourceWaitService);

    it('should remove an already lazy loaded element and return true', () => {
      const script = document.createElement('script');
      document.body.appendChild(script);
      service.loaded.set('x', script);

      expect(document.body.lastElementChild).toBe(script);

      const result = service.remove('x');

      expect(document.body.lastElementChild).toBeNull();
      expect(service.loaded.has('x')).toBe(false);
      expect(result).toBe(true);
    });

    it('should return false when path not found', () => {
      service.loaded.set('foo', null);

      const result = service.remove('bar');

      expect(result).toBe(false);
    });
  });
});
