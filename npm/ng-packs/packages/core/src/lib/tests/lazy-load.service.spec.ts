import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { LazyLoadService } from '../services/lazy-load.service';
import { ScriptLoadingStrategy } from '../strategies';

describe('LazyLoadService', () => {
  describe('#load', () => {
    const service = new LazyLoadService();
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
      service.loaded.add(strategy.path);

      service.load(strategy).subscribe(event => {
        expect(event).toEqual(loadEvent);
        done();
      });
    });
  });
});

describe('LazyLoadService (Deprecated)', () => {
  let spectator: SpectatorService<LazyLoadService>;
  let service: LazyLoadService;
  const scriptElement = document.createElement('script');
  const linkElement = document.createElement('link');
  const styleElement = document.createElement('style');
  const cloneDocument = { ...document };

  const createService = createServiceFactory({ service: LazyLoadService });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  afterEach(() => (document = { ...cloneDocument }));

  test('should load script with content just one time', done => {
    const spy = jest.spyOn(document, 'createElement');
    spy.mockReturnValue(scriptElement);

    service.load('https://abp.io', 'script', 'test').subscribe(res => {
      expect(
        document.querySelector('script[src="https://abp.io"][type="text/javascript"]').textContent,
      ).toMatch('test');
    });

    scriptElement.onload(null);

    service.load('https://abp.io', 'script', 'test').subscribe(res => {
      expect(
        document.querySelectorAll('script[src="https://abp.io"][type="text/javascript"]'),
      ).toHaveLength(1);
      done();
    });
  });

  test('should load style element', done => {
    const spy = jest.spyOn(document, 'createElement');
    spy.mockReturnValue(styleElement);

    const content = '* { color: black; }';
    service.load(null, 'style', content).subscribe(res => {
      expect(document.querySelector('style').textContent).toMatch(content);
      done();
    });

    styleElement.onload(null);
  });

  describe('style with url', () => {
    beforeEach(() => {
      const spy = jest.spyOn(document, 'createElement');
      spy.mockReturnValue(linkElement);
    });

    test('should load an link element', done => {
      service.load('https://abp.io', 'style').subscribe(res => {
        expect(
          document.querySelector('link[type="text/css"][rel="stylesheet"][href="https://abp.io"]'),
        ).toBeTruthy();
        done();
      });

      linkElement.onload(null);
    });

    test('should load link elements', done => {
      service.load(['https://abp.io', 'https://volosoft.com'], 'style').subscribe(res => {
        expect(document.querySelector('link[href="https://volosoft.com"]')).toBeTruthy();
        done();
      });

      linkElement.onload(null);
    });
  });

  test('should throw error when required parameters are null', done => {
    service
      .load(null, 'style')
      .pipe(
        catchError(err => {
          expect(err).toBeTruthy();
          done();
          return of(null);
        }),
      )
      .subscribe();
  });
});
