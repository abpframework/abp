import { Injectable } from '@angular/core';
import { concat, Observable, of, throwError } from 'rxjs';
import { delay, retryWhen, shareReplay, take, tap } from 'rxjs/operators';
import { LoadingStrategy } from '../strategies';

@Injectable({
  providedIn: 'root',
})
export class LazyLoadService {
  readonly loaded = new Map<string, HTMLScriptElement | HTMLLinkElement>();

  load(strategy: LoadingStrategy, retryTimes?: number, retryDelay?: number): Observable<Event> {
    if (this.loaded.has(strategy.path)) return of(new CustomEvent('load'));

    return strategy.createStream().pipe(
      retryWhen(error$ =>
        concat(
          error$.pipe(delay(retryDelay), take(retryTimes)),
          throwError(new CustomEvent('error')),
        ),
      ),
      tap(() => this.loaded.set(strategy.path, strategy.element)),
      delay(100),
      shareReplay({ bufferSize: 1, refCount: true }),
    );
  }

  remove(path: string): boolean {
    const element = this.loaded.get(path);

    if (!element) return false;

    element.parentNode.removeChild(element);
    this.loaded.delete(path);
    return true;
  }
}
