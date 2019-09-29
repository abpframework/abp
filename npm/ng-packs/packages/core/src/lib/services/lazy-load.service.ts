import { Inject, Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { uuid } from '../utils';

@Injectable({
  providedIn: 'root',
})
export class LazyLoadService {
  loadedLibraries: { [url: string]: ReplaySubject<void> } = {};

  load(
    url: string,
    type: 'script' | 'style',
    content: string = '',
    targetQuery: string = 'body',
    position: InsertPosition = 'afterend',
  ): Observable<void> {
    if (!url && !content) return;
    const key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();

    if (this.loadedLibraries[key]) {
      return this.loadedLibraries[key].asObservable();
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
    };

    document.querySelector(targetQuery).insertAdjacentElement(position, library);

    return this.loadedLibraries[key].asObservable();
  }
}
