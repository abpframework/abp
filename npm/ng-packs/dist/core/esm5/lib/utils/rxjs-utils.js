/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
                componentInstance['__takeUntilDestroy'].next(true);
                componentInstance['__takeUntilDestroy'].complete();
            });
        }
        return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
    });
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7Ozs7QUFFM0MsU0FBUyxVQUFVLENBQUMsS0FBSztJQUN2QixPQUFPLE9BQU8sS0FBSyxLQUFLLFVBQVUsQ0FBQztBQUNyQyxDQUFDOztBQUVELE1BQU0sS0FBTyxnQkFBZ0I7Ozs7O0FBQUcsVUFBQyxpQkFBaUIsRUFBRSxpQkFBaUM7SUFBakMsa0NBQUEsRUFBQSxpQ0FBaUM7Ozs7OztJQUFLLFVBQ3hGLE1BQXFCOztZQUVmLGVBQWUsR0FBRyxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQztRQUM1RCxJQUFJLFVBQVUsQ0FBQyxlQUFlLENBQUMsS0FBSyxLQUFLLEVBQUU7WUFDekMsTUFBTSxJQUFJLEtBQUssQ0FDVixpQkFBaUIsQ0FBQyxXQUFXLENBQUMsSUFBSSx1REFBa0QsaUJBQW1CLENBQzNHLENBQUM7U0FDSDtRQUNELElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFO1lBQzVDLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztZQUV4RCxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQzs7O1lBQUc7Z0JBQ3JDLFVBQVUsQ0FBQyxlQUFlLENBQUMsSUFBSSxlQUFlLENBQUMsS0FBSyxDQUFDLElBQUksRUFBRSxTQUFTLENBQUMsQ0FBQztnQkFDdEUsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7Z0JBQ25ELGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7WUFDckQsQ0FBQyxDQUFBLENBQUM7U0FDSDtRQUNELE9BQU8sTUFBTSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUksaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDNUUsQ0FBQztDQUFBLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyB0YWtlVW50aWwgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbmZ1bmN0aW9uIGlzRnVuY3Rpb24odmFsdWUpIHtcbiAgcmV0dXJuIHR5cGVvZiB2YWx1ZSA9PT0gJ2Z1bmN0aW9uJztcbn1cblxuZXhwb3J0IGNvbnN0IHRha2VVbnRpbERlc3Ryb3kgPSAoY29tcG9uZW50SW5zdGFuY2UsIGRlc3Ryb3lNZXRob2ROYW1lID0gJ25nT25EZXN0cm95JykgPT4gPFQ+KFxuICBzb3VyY2U6IE9ic2VydmFibGU8VD4sXG4pID0+IHtcbiAgY29uc3Qgb3JpZ2luYWxEZXN0cm95ID0gY29tcG9uZW50SW5zdGFuY2VbZGVzdHJveU1ldGhvZE5hbWVdO1xuICBpZiAoaXNGdW5jdGlvbihvcmlnaW5hbERlc3Ryb3kpID09PSBmYWxzZSkge1xuICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgIGAke2NvbXBvbmVudEluc3RhbmNlLmNvbnN0cnVjdG9yLm5hbWV9IGlzIHVzaW5nIHVudGlsRGVzdHJveWVkIGJ1dCBkb2Vzbid0IGltcGxlbWVudCAke2Rlc3Ryb3lNZXRob2ROYW1lfWAsXG4gICAgKTtcbiAgfVxuICBpZiAoIWNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkge1xuICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSA9IG5ldyBTdWJqZWN0KCk7XG5cbiAgICBjb21wb25lbnRJbnN0YW5jZVtkZXN0cm95TWV0aG9kTmFtZV0gPSBmdW5jdGlvbigpIHtcbiAgICAgIGlzRnVuY3Rpb24ob3JpZ2luYWxEZXN0cm95KSAmJiBvcmlnaW5hbERlc3Ryb3kuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5uZXh0KHRydWUpO1xuICAgICAgY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddLmNvbXBsZXRlKCk7XG4gICAgfTtcbiAgfVxuICByZXR1cm4gc291cmNlLnBpcGUodGFrZVVudGlsPFQ+KGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkpO1xufTtcbiJdfQ==