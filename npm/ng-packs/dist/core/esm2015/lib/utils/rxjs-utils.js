/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/rxjs-utils.ts
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
export const takeUntilDestroy = (/**
 * @param {?} componentInstance
 * @param {?=} destroyMethodName
 * @return {?}
 */
(componentInstance, destroyMethodName = 'ngOnDestroy') => (/**
 * @template T
 * @param {?} source
 * @return {?}
 */
(source) => {
    /** @type {?} */
    const originalDestroy = componentInstance[destroyMethodName];
    if (isFunction(originalDestroy) === false) {
        throw new Error(`${componentInstance.constructor.name} is using untilDestroyed but doesn't implement ${destroyMethodName}`);
    }
    if (!componentInstance['__takeUntilDestroy']) {
        componentInstance['__takeUntilDestroy'] = new Subject();
        componentInstance[destroyMethodName] = (/**
         * @return {?}
         */
        function () {
            // tslint:disable-next-line: no-unused-expression
            isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
            componentInstance['__takeUntilDestroy'].next(true);
            componentInstance['__takeUntilDestroy'].complete();
        });
    }
    return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMzQyxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7Ozs7O0FBRTNDLFNBQVMsVUFBVSxDQUFDLEtBQUs7SUFDdkIsT0FBTyxPQUFPLEtBQUssS0FBSyxVQUFVLENBQUM7QUFDckMsQ0FBQzs7QUFFRCxNQUFNLE9BQU8sZ0JBQWdCOzs7OztBQUFHLENBQUMsaUJBQWlCLEVBQUUsaUJBQWlCLEdBQUcsYUFBYSxFQUFFLEVBQUU7Ozs7O0FBQUMsQ0FDeEYsTUFBcUIsRUFDckIsRUFBRTs7VUFDSSxlQUFlLEdBQUcsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7SUFDNUQsSUFBSSxVQUFVLENBQUMsZUFBZSxDQUFDLEtBQUssS0FBSyxFQUFFO1FBQ3pDLE1BQU0sSUFBSSxLQUFLLENBQ2IsR0FBRyxpQkFBaUIsQ0FBQyxXQUFXLENBQUMsSUFBSSxrREFBa0QsaUJBQWlCLEVBQUUsQ0FDM0csQ0FBQztLQUNIO0lBQ0QsSUFBSSxDQUFDLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLEVBQUU7UUFDNUMsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsR0FBRyxJQUFJLE9BQU8sRUFBRSxDQUFDO1FBRXhELGlCQUFpQixDQUFDLGlCQUFpQixDQUFDOzs7UUFBRztZQUNyQyxpREFBaUQ7WUFDakQsVUFBVSxDQUFDLGVBQWUsQ0FBQyxJQUFJLGVBQWUsQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFFLFNBQVMsQ0FBQyxDQUFDO1lBQ3RFLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO1lBQ25ELGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7UUFDckQsQ0FBQyxDQUFBLENBQUM7S0FDSDtJQUNELE9BQU8sTUFBTSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUksaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDNUUsQ0FBQyxDQUFBLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHRha2VVbnRpbCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuXHJcbmZ1bmN0aW9uIGlzRnVuY3Rpb24odmFsdWUpIHtcclxuICByZXR1cm4gdHlwZW9mIHZhbHVlID09PSAnZnVuY3Rpb24nO1xyXG59XHJcblxyXG5leHBvcnQgY29uc3QgdGFrZVVudGlsRGVzdHJveSA9IChjb21wb25lbnRJbnN0YW5jZSwgZGVzdHJveU1ldGhvZE5hbWUgPSAnbmdPbkRlc3Ryb3knKSA9PiA8VD4oXHJcbiAgc291cmNlOiBPYnNlcnZhYmxlPFQ+XHJcbikgPT4ge1xyXG4gIGNvbnN0IG9yaWdpbmFsRGVzdHJveSA9IGNvbXBvbmVudEluc3RhbmNlW2Rlc3Ryb3lNZXRob2ROYW1lXTtcclxuICBpZiAoaXNGdW5jdGlvbihvcmlnaW5hbERlc3Ryb3kpID09PSBmYWxzZSkge1xyXG4gICAgdGhyb3cgbmV3IEVycm9yKFxyXG4gICAgICBgJHtjb21wb25lbnRJbnN0YW5jZS5jb25zdHJ1Y3Rvci5uYW1lfSBpcyB1c2luZyB1bnRpbERlc3Ryb3llZCBidXQgZG9lc24ndCBpbXBsZW1lbnQgJHtkZXN0cm95TWV0aG9kTmFtZX1gXHJcbiAgICApO1xyXG4gIH1cclxuICBpZiAoIWNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkge1xyXG4gICAgY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddID0gbmV3IFN1YmplY3QoKTtcclxuXHJcbiAgICBjb21wb25lbnRJbnN0YW5jZVtkZXN0cm95TWV0aG9kTmFtZV0gPSBmdW5jdGlvbigpIHtcclxuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxyXG4gICAgICBpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgJiYgb3JpZ2luYWxEZXN0cm95LmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XHJcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5uZXh0KHRydWUpO1xyXG4gICAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10uY29tcGxldGUoKTtcclxuICAgIH07XHJcbiAgfVxyXG4gIHJldHVybiBzb3VyY2UucGlwZSh0YWtlVW50aWw8VD4oY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddKSk7XHJcbn07XHJcbiJdfQ==