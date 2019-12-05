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
export var takeUntilDestroy = (/**
 * @param {?} componentInstance
 * @param {?=} destroyMethodName
 * @return {?}
 */
function (componentInstance, destroyMethodName) {
    if (destroyMethodName === void 0) { destroyMethodName = 'ngOnDestroy'; }
    return (/**
     * @template T
     * @param {?} source
     * @return {?}
     */
    function (source) {
        /** @type {?} */
        var originalDestroy = componentInstance[destroyMethodName];
        if (isFunction(originalDestroy) === false) {
            throw new Error(componentInstance.constructor.name + " is using untilDestroyed but doesn't implement " + destroyMethodName);
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
    });
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMzQyxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7Ozs7O0FBRTNDLFNBQVMsVUFBVSxDQUFDLEtBQUs7SUFDdkIsT0FBTyxPQUFPLEtBQUssS0FBSyxVQUFVLENBQUM7QUFDckMsQ0FBQzs7QUFFRCxNQUFNLEtBQU8sZ0JBQWdCOzs7OztBQUFHLFVBQUMsaUJBQWlCLEVBQUUsaUJBQWlDO0lBQWpDLGtDQUFBLEVBQUEsaUNBQWlDOzs7Ozs7SUFBSyxVQUN4RixNQUFxQjs7WUFFZixlQUFlLEdBQUcsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7UUFDNUQsSUFBSSxVQUFVLENBQUMsZUFBZSxDQUFDLEtBQUssS0FBSyxFQUFFO1lBQ3pDLE1BQU0sSUFBSSxLQUFLLENBQ1YsaUJBQWlCLENBQUMsV0FBVyxDQUFDLElBQUksdURBQWtELGlCQUFtQixDQUMzRyxDQUFDO1NBQ0g7UUFDRCxJQUFJLENBQUMsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsRUFBRTtZQUM1QyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxHQUFHLElBQUksT0FBTyxFQUFFLENBQUM7WUFFeEQsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7OztZQUFHO2dCQUNyQyxpREFBaUQ7Z0JBQ2pELFVBQVUsQ0FBQyxlQUFlLENBQUMsSUFBSSxlQUFlLENBQUMsS0FBSyxDQUFDLElBQUksRUFBRSxTQUFTLENBQUMsQ0FBQztnQkFDdEUsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7Z0JBQ25ELGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7WUFDckQsQ0FBQyxDQUFBLENBQUM7U0FDSDtRQUNELE9BQU8sTUFBTSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUksaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDNUUsQ0FBQztDQUFBLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWwgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbmZ1bmN0aW9uIGlzRnVuY3Rpb24odmFsdWUpIHtcbiAgcmV0dXJuIHR5cGVvZiB2YWx1ZSA9PT0gJ2Z1bmN0aW9uJztcbn1cblxuZXhwb3J0IGNvbnN0IHRha2VVbnRpbERlc3Ryb3kgPSAoY29tcG9uZW50SW5zdGFuY2UsIGRlc3Ryb3lNZXRob2ROYW1lID0gJ25nT25EZXN0cm95JykgPT4gPFQ+KFxuICBzb3VyY2U6IE9ic2VydmFibGU8VD5cbikgPT4ge1xuICBjb25zdCBvcmlnaW5hbERlc3Ryb3kgPSBjb21wb25lbnRJbnN0YW5jZVtkZXN0cm95TWV0aG9kTmFtZV07XG4gIGlmIChpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgPT09IGZhbHNlKSB7XG4gICAgdGhyb3cgbmV3IEVycm9yKFxuICAgICAgYCR7Y29tcG9uZW50SW5zdGFuY2UuY29uc3RydWN0b3IubmFtZX0gaXMgdXNpbmcgdW50aWxEZXN0cm95ZWQgYnV0IGRvZXNuJ3QgaW1wbGVtZW50ICR7ZGVzdHJveU1ldGhvZE5hbWV9YFxuICAgICk7XG4gIH1cbiAgaWYgKCFjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10pIHtcbiAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10gPSBuZXcgU3ViamVjdCgpO1xuXG4gICAgY29tcG9uZW50SW5zdGFuY2VbZGVzdHJveU1ldGhvZE5hbWVdID0gZnVuY3Rpb24oKSB7XG4gICAgICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLXVudXNlZC1leHByZXNzaW9uXG4gICAgICBpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgJiYgb3JpZ2luYWxEZXN0cm95LmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10ubmV4dCh0cnVlKTtcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5jb21wbGV0ZSgpO1xuICAgIH07XG4gIH1cbiAgcmV0dXJuIHNvdXJjZS5waXBlKHRha2VVbnRpbDxUPihjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10pKTtcbn07XG4iXX0=