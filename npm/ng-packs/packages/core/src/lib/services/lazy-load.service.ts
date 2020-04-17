import { Injectable } from '@angular/core';
import { concat, Observable, of, ReplaySubject, throwError } from 'rxjs';
import { delay, retryWhen, shareReplay, take, tap } from 'rxjs/operators';
import { LoadingStrategy } from '../strategies';
import { uuid } from '../utils';

@Injectable({
  providedIn: 'root',
})
export class LazyLoadService {
  readonly loaded = new Set();

  loadedLibraries: { [url: string]: ReplaySubject<void> } = {};

  load(strategy: LoadingStrategy, retryTimes?: number, retryDelay?: number): Observable<Event>;
  /**
   *
   * @deprecated Use other overload that requires a strategy as first param
   */
  load(
    urlOrUrls: string | string[],
    type: 'script' | 'style',
    content?: string,
    targetQuery?: string,
    position?: InsertPosition,
  ): Observable<void>;
  load(
    strategyOrUrl: LoadingStrategy | string | string[],
    retryTimesOrType?: number | 'script' | 'style',
    retryDelayOrContent?: number | string,
    targetQuery: string = 'body',
    position: InsertPosition = 'beforeend',
  ): Observable<Event | void> {
    if (strategyOrUrl instanceof LoadingStrategy) {
      const strategy = strategyOrUrl;
      const retryTimes = typeof retryTimesOrType === 'number' ? retryTimesOrType : 2;
      const retryDelay = typeof retryDelayOrContent === 'number' ? retryDelayOrContent : 1000;

      if (this.loaded.has(strategy.path)) return of(new CustomEvent('load'));

      return strategy.createStream().pipe(
        retryWhen(error$ =>
          concat(
            error$.pipe(delay(retryDelay), take(retryTimes)),
            throwError(new CustomEvent('error')),
          ),
        ),
        tap(() => this.loaded.add(strategy.path)),
        delay(100),
        shareReplay({ bufferSize: 1, refCount: true }),
      );
    }

    let urlOrUrls = strategyOrUrl;
    const content = (retryDelayOrContent as string) || '';
    const type = retryTimesOrType as 'script' | 'style';

    if (!urlOrUrls && !content) {
      return throwError('Should pass url or content');
    } else if (!urlOrUrls && content) {
      urlOrUrls = [null];
    }

    if (!Array.isArray(urlOrUrls)) {
      urlOrUrls = [urlOrUrls];
    }

    return new Observable(subscriber => {
      (urlOrUrls as string[]).forEach((url, index) => {
        const key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();

        if (this.loadedLibraries[key]) {
          subscriber.next();
          subscriber.complete();
          return;
        }

        this.loadedLibraries[key] = new ReplaySubject();

        let library;
        if (type === 'script') {
          library = document.createElement('script');
          library.type = 'text/javascript';
          if (url) {
            (library as HTMLScriptElement).src = url;
          }

          (library as HTMLScriptElement).text = content;
        } else if (url) {
          library = document.createElement('link');
          library.type = 'text/css';
          (library as HTMLLinkElement).rel = 'stylesheet';

          if (url) {
            (library as HTMLLinkElement).href = url;
          }
        } else {
          library = document.createElement('style');
          (library as HTMLStyleElement).textContent = content;
        }

        library.onload = () => {
          this.loadedLibraries[key].next();
          this.loadedLibraries[key].complete();

          if (index === urlOrUrls.length - 1) {
            subscriber.next();
            subscriber.complete();
          }
        };

        document.querySelector(targetQuery).insertAdjacentElement(position, library);
      });
    });
  }
}
