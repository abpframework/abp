/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
/**
 * @param {?} value
 * @return {?}
 */
function isFunction(value) {
  return typeof value === 'function';
}
/** @type {?} */
export var takeUntilDestroy
/**
 * @param {?} componentInstance
 * @param {?=} destroyMethodName
 * @return {?}
 */ = (function(componentInstance, destroyMethodName) {
  if (destroyMethodName === void 0) {
    destroyMethodName = 'ngOnDestroy';
  }
  return (
    /**
     * @template T
     * @param {?} source
     * @return {?}
     */
    function(source) {
      /** @type {?} */
      var originalDestroy = componentInstance[destroyMethodName];
      if (isFunction(originalDestroy) === false) {
        throw new Error(
          componentInstance.constructor.name + " is using untilDestroyed but doesn't implement " + destroyMethodName,
        );
      }
      if (!componentInstance['__takeUntilDestroy']) {
        componentInstance['__takeUntilDestroy'] = new Subject();
        componentInstance[destroyMethodName]
        /**
         * @return {?}
         */ = function() {
          // tslint:disable-next-line: no-unused-expression
          isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
          componentInstance['__takeUntilDestroy'].next(true);
          componentInstance['__takeUntilDestroy'].complete();
        };
      }
      return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
    }
  );
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7Ozs7QUFFM0MsU0FBUyxVQUFVLENBQUMsS0FBSztJQUN2QixPQUFPLE9BQU8sS0FBSyxLQUFLLFVBQVUsQ0FBQztBQUNyQyxDQUFDOztBQUVELE1BQU0sS0FBTyxnQkFBZ0I7Ozs7O0FBQUcsVUFBQyxpQkFBaUIsRUFBRSxpQkFBaUM7SUFBakMsa0NBQUEsRUFBQSxpQ0FBaUM7Ozs7OztJQUFLLFVBQ3hGLE1BQXFCOztZQUVmLGVBQWUsR0FBRyxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQztRQUM1RCxJQUFJLFVBQVUsQ0FBQyxlQUFlLENBQUMsS0FBSyxLQUFLLEVBQUU7WUFDekMsTUFBTSxJQUFJLEtBQUssQ0FDVixpQkFBaUIsQ0FBQyxXQUFXLENBQUMsSUFBSSx1REFBa0QsaUJBQW1CLENBQzNHLENBQUM7U0FDSDtRQUNELElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFO1lBQzVDLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztZQUV4RCxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQzs7O1lBQUc7Z0JBQ3JDLGlEQUFpRDtnQkFDakQsVUFBVSxDQUFDLGVBQWUsQ0FBQyxJQUFJLGVBQWUsQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFFLFNBQVMsQ0FBQyxDQUFDO2dCQUN0RSxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztnQkFDbkQsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztZQUNyRCxDQUFDLENBQUEsQ0FBQztTQUNIO1FBQ0QsT0FBTyxNQUFNLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBSSxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUM1RSxDQUFDO0NBQUEsQ0FBQSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRha2VVbnRpbCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuZnVuY3Rpb24gaXNGdW5jdGlvbih2YWx1ZSkge1xuICByZXR1cm4gdHlwZW9mIHZhbHVlID09PSAnZnVuY3Rpb24nO1xufVxuXG5leHBvcnQgY29uc3QgdGFrZVVudGlsRGVzdHJveSA9IChjb21wb25lbnRJbnN0YW5jZSwgZGVzdHJveU1ldGhvZE5hbWUgPSAnbmdPbkRlc3Ryb3knKSA9PiA8VD4oXG4gIHNvdXJjZTogT2JzZXJ2YWJsZTxUPlxuKSA9PiB7XG4gIGNvbnN0IG9yaWdpbmFsRGVzdHJveSA9IGNvbXBvbmVudEluc3RhbmNlW2Rlc3Ryb3lNZXRob2ROYW1lXTtcbiAgaWYgKGlzRnVuY3Rpb24ob3JpZ2luYWxEZXN0cm95KSA9PT0gZmFsc2UpIHtcbiAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICBgJHtjb21wb25lbnRJbnN0YW5jZS5jb25zdHJ1Y3Rvci5uYW1lfSBpcyB1c2luZyB1bnRpbERlc3Ryb3llZCBidXQgZG9lc24ndCBpbXBsZW1lbnQgJHtkZXN0cm95TWV0aG9kTmFtZX1gXG4gICAgKTtcbiAgfVxuICBpZiAoIWNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkge1xuICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSA9IG5ldyBTdWJqZWN0KCk7XG5cbiAgICBjb21wb25lbnRJbnN0YW5jZVtkZXN0cm95TWV0aG9kTmFtZV0gPSBmdW5jdGlvbigpIHtcbiAgICAgIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tdW51c2VkLWV4cHJlc3Npb25cbiAgICAgIGlzRnVuY3Rpb24ob3JpZ2luYWxEZXN0cm95KSAmJiBvcmlnaW5hbERlc3Ryb3kuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5uZXh0KHRydWUpO1xuICAgICAgY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddLmNvbXBsZXRlKCk7XG4gICAgfTtcbiAgfVxuICByZXR1cm4gc291cmNlLnBpcGUodGFrZVVudGlsPFQ+KGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkpO1xufTtcbiJdfQ==
