import { Observable, Observer } from 'rxjs';
import {
  CrossOriginStrategy,
  CROSS_ORIGIN_STRATEGY,
  DomStrategy,
  DOM_STRATEGY,
} from '../strategies';

export function fromLazyLoad<T extends Event>(
  element: HTMLScriptElement | HTMLLinkElement,
  domStrategy: DomStrategy = DOM_STRATEGY.AppendToHead(),
  crossOriginStrategy: CrossOriginStrategy = CROSS_ORIGIN_STRATEGY.Anonymous(),
): Observable<T> {
  crossOriginStrategy.setCrossOrigin(element);
  domStrategy.insertElement(element);

  return Observable.create((observer: Observer<Event>) => {
    element.onload = event => {
      clearCallbacks(element);
      observer.next(event);
      observer.complete();
    };

    const handleError = createErrorHandler(observer, element);

    element.onerror = handleError;
    element.onabort = handleError;
    element.onemptied = handleError;
    element.onstalled = handleError;
    element.onsuspend = handleError;

    return () => {
      clearCallbacks(element);
      observer.complete();
    };
  });
}

function createErrorHandler(observer: Observer<Event>, element: HTMLElement) {
  return function(event: Event | string) {
    clearCallbacks(element);
    element.parentNode.removeChild(element);
    observer.error(event);
  };
}

function clearCallbacks(element: HTMLElement) {
  element.onload = null;
  element.onerror = null;
  element.onabort = null;
  element.onemptied = null;
  element.onstalled = null;
  element.onsuspend = null;
}
