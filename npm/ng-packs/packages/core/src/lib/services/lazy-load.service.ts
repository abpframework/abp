import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { uuid } from '../utils';

@Injectable({
  providedIn: 'root',
})
export class LazyLoadService {
  loadedLibraries: { [url: string]: ReplaySubject<void> } = {};

  load(
    urlOrUrls: string | string[],
    type: 'script' | 'style',
    content: string = '',
    targetQuery: string = 'body',
    position: InsertPosition = 'afterend',
  ): Observable<void> {
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
