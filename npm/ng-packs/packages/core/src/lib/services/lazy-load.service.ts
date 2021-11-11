import { Injectable } from '@angular/core';
import { concat, Observable, of, throwError } from 'rxjs';
import { delay, retryWhen, shareReplay, take, tap } from 'rxjs/operators';
import { LoadingStrategy } from '../strategies';
import { ResourceWaitService } from './resource-wait.service';

@Injectable({
  providedIn: 'root',
})
export class LazyLoadService {
  readonly loaded = new Map<string, HTMLScriptElement | HTMLLinkElement | null>();

  constructor(private resourceWaitService: ResourceWaitService) {}

  load(strategy: LoadingStrategy, retryTimes?: number, retryDelay?: number): Observable<Event> {
    if (this.loaded.has(strategy.path)) return of(new CustomEvent('load'));
    this.resourceWaitService.addResource(strategy.path);
    return strategy.createStream().pipe(
      retryWhen(error$ =>
        concat(
          error$.pipe(delay(retryDelay), take(retryTimes)),
          throwError(new CustomEvent('error')),
        ),
      ),
      tap(() => {
        this.loaded.set(strategy.path, strategy.element);
        this.resourceWaitService.deleteResource(strategy.path);
      }),
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
