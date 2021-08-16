// tslint:disable: max-line-length
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

function isFunction(value) {
  return typeof value === 'function';
}

/**
 * @deprecated no longer working, please use SubscriptionService (https://docs.abp.io/en/abp/latest/UI/Angular/Subscription-Service) instead.
 */
export const takeUntilDestroy =
  (componentInstance, destroyMethodName = 'ngOnDestroy') =>
  <T>(source: Observable<T>) => {
    const originalDestroy = componentInstance[destroyMethodName];
    if (isFunction(originalDestroy) === false) {
      throw new Error(
        `${componentInstance.constructor.name} is using untilDestroyed but doesn't implement ${destroyMethodName}`,
      );
    }
    if (!componentInstance['__takeUntilDestroy']) {
      componentInstance['__takeUntilDestroy'] = new Subject();

      componentInstance[destroyMethodName] = function () {
        // eslint-disable-next-line prefer-rest-params
        isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
        componentInstance['__takeUntilDestroy'].next(true);
        componentInstance['__takeUntilDestroy'].complete();
      };
    }
    return source.pipe(takeUntil<T>(componentInstance['__takeUntilDestroy']));
  };
