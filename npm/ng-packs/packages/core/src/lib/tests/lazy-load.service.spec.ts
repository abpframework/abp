import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import clone from 'just-clone';
import { LazyLoadService } from '../services/lazy-load.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

describe('LazyLoadService', () => {
  let spectator: SpectatorService<LazyLoadService>;
  let service: LazyLoadService;
  const scriptElement = document.createElement('script');
  const linkElement = document.createElement('link');
  const styleElement = document.createElement('style');
  const cloneDocument = clone(document);

  const createService = createServiceFactory({ service: LazyLoadService });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  afterEach(() => (document = clone(cloneDocument)));

  test('should load script with content just one time', done => {
    const spy = jest.spyOn(document, 'createElement');
    spy.mockReturnValue(scriptElement);

    service.load('https://abp.io', 'script', 'test').subscribe(res => {
      expect(document.querySelector('script[src="https://abp.io"][type="text/javascript"]').textContent).toMatch(
        'test',
      );
    });

    scriptElement.onload(null);

    service.load('https://abp.io', 'script', 'test').subscribe(res => {
      expect(document.querySelectorAll('script[src="https://abp.io"][type="text/javascript"]')).toHaveLength(1);
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
        expect(document.querySelector('link[type="text/css"][rel="stylesheet"][href="https://abp.io"]')).toBeTruthy();
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
