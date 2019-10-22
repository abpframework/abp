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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7Ozs7QUFFM0MsU0FBUyxVQUFVLENBQUMsS0FBSztJQUN2QixPQUFPLE9BQU8sS0FBSyxLQUFLLFVBQVUsQ0FBQztBQUNyQyxDQUFDOztBQUVELE1BQU0sS0FBTyxnQkFBZ0I7Ozs7O0FBQUcsVUFBQyxpQkFBaUIsRUFBRSxpQkFBaUM7SUFBakMsa0NBQUEsRUFBQSxpQ0FBaUM7Ozs7OztJQUFLLFVBQ3hGLE1BQXFCOztZQUVmLGVBQWUsR0FBRyxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQztRQUM1RCxJQUFJLFVBQVUsQ0FBQyxlQUFlLENBQUMsS0FBSyxLQUFLLEVBQUU7WUFDekMsTUFBTSxJQUFJLEtBQUssQ0FDVixpQkFBaUIsQ0FBQyxXQUFXLENBQUMsSUFBSSx1REFBa0QsaUJBQW1CLENBQzNHLENBQUM7U0FDSDtRQUNELElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFO1lBQzVDLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztZQUV4RCxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQzs7O1lBQUc7Z0JBQ3JDLGlEQUFpRDtnQkFDakQsVUFBVSxDQUFDLGVBQWUsQ0FBQyxJQUFJLGVBQWUsQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFFLFNBQVMsQ0FBQyxDQUFDO2dCQUN0RSxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztnQkFDbkQsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQztZQUNyRCxDQUFDLENBQUEsQ0FBQztTQUNIO1FBQ0QsT0FBTyxNQUFNLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBSSxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUM1RSxDQUFDO0NBQUEsQ0FBQSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuZnVuY3Rpb24gaXNGdW5jdGlvbih2YWx1ZSkge1xyXG4gIHJldHVybiB0eXBlb2YgdmFsdWUgPT09ICdmdW5jdGlvbic7XHJcbn1cclxuXHJcbmV4cG9ydCBjb25zdCB0YWtlVW50aWxEZXN0cm95ID0gKGNvbXBvbmVudEluc3RhbmNlLCBkZXN0cm95TWV0aG9kTmFtZSA9ICduZ09uRGVzdHJveScpID0+IDxUPihcclxuICBzb3VyY2U6IE9ic2VydmFibGU8VD5cclxuKSA9PiB7XHJcbiAgY29uc3Qgb3JpZ2luYWxEZXN0cm95ID0gY29tcG9uZW50SW5zdGFuY2VbZGVzdHJveU1ldGhvZE5hbWVdO1xyXG4gIGlmIChpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgPT09IGZhbHNlKSB7XHJcbiAgICB0aHJvdyBuZXcgRXJyb3IoXHJcbiAgICAgIGAke2NvbXBvbmVudEluc3RhbmNlLmNvbnN0cnVjdG9yLm5hbWV9IGlzIHVzaW5nIHVudGlsRGVzdHJveWVkIGJ1dCBkb2Vzbid0IGltcGxlbWVudCAke2Rlc3Ryb3lNZXRob2ROYW1lfWBcclxuICAgICk7XHJcbiAgfVxyXG4gIGlmICghY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddKSB7XHJcbiAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10gPSBuZXcgU3ViamVjdCgpO1xyXG5cclxuICAgIGNvbXBvbmVudEluc3RhbmNlW2Rlc3Ryb3lNZXRob2ROYW1lXSA9IGZ1bmN0aW9uKCkge1xyXG4gICAgICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLXVudXNlZC1leHByZXNzaW9uXHJcbiAgICAgIGlzRnVuY3Rpb24ob3JpZ2luYWxEZXN0cm95KSAmJiBvcmlnaW5hbERlc3Ryb3kuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcclxuICAgICAgY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddLm5leHQodHJ1ZSk7XHJcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5jb21wbGV0ZSgpO1xyXG4gICAgfTtcclxuICB9XHJcbiAgcmV0dXJuIHNvdXJjZS5waXBlKHRha2VVbnRpbDxUPihjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10pKTtcclxufTtcclxuIl19