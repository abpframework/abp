import { Injectable } from '@angular/core';
import { concat, Observable, of, pipe, throwError } from 'rxjs';
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
    const delayOperator = retryDelay ? pipe(delay(retryDelay)) : pipe();
    const takeOp = retryTimes ? pipe(take(retryTimes)) : pipe();
    return strategy.createStream().pipe(
      retryWhen(error$ =>
        concat(
          error$.pipe(delayOperator, takeOp),
          throwError(() => new CustomEvent('error')),
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

    element.parentNode?.removeChild(element);
    this.loaded.delete(path);
    return true;
  }
}
