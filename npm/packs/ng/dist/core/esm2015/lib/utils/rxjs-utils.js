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
            isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
            componentInstance['__takeUntilDestroy'].next(true);
            componentInstance['__takeUntilDestroy'].complete();
        });
    }
    return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7Ozs7QUFFM0MsU0FBUyxVQUFVLENBQUMsS0FBSztJQUN2QixPQUFPLE9BQU8sS0FBSyxLQUFLLFVBQVUsQ0FBQztBQUNyQyxDQUFDOztBQUVELE1BQU0sT0FBTyxnQkFBZ0I7Ozs7O0FBQUcsQ0FBQyxpQkFBaUIsRUFBRSxpQkFBaUIsR0FBRyxhQUFhLEVBQUUsRUFBRTs7Ozs7QUFBQyxDQUN4RixNQUFxQixFQUNyQixFQUFFOztVQUNJLGVBQWUsR0FBRyxpQkFBaUIsQ0FBQyxpQkFBaUIsQ0FBQztJQUM1RCxJQUFJLFVBQVUsQ0FBQyxlQUFlLENBQUMsS0FBSyxLQUFLLEVBQUU7UUFDekMsTUFBTSxJQUFJLEtBQUssQ0FDYixHQUFHLGlCQUFpQixDQUFDLFdBQVcsQ0FBQyxJQUFJLGtEQUFrRCxpQkFBaUIsRUFBRSxDQUMzRyxDQUFDO0tBQ0g7SUFDRCxJQUFJLENBQUMsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsRUFBRTtRQUM1QyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxHQUFHLElBQUksT0FBTyxFQUFFLENBQUM7UUFFeEQsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7OztRQUFHO1lBQ3JDLFVBQVUsQ0FBQyxlQUFlLENBQUMsSUFBSSxlQUFlLENBQUMsS0FBSyxDQUFDLElBQUksRUFBRSxTQUFTLENBQUMsQ0FBQztZQUN0RSxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztZQUNuRCxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLFFBQVEsRUFBRSxDQUFDO1FBQ3JELENBQUMsQ0FBQSxDQUFDO0tBQ0g7SUFDRCxPQUFPLE1BQU0sQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFJLGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsQ0FBQyxDQUFDO0FBQzVFLENBQUMsQ0FBQSxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5mdW5jdGlvbiBpc0Z1bmN0aW9uKHZhbHVlKSB7XG4gIHJldHVybiB0eXBlb2YgdmFsdWUgPT09ICdmdW5jdGlvbic7XG59XG5cbmV4cG9ydCBjb25zdCB0YWtlVW50aWxEZXN0cm95ID0gKGNvbXBvbmVudEluc3RhbmNlLCBkZXN0cm95TWV0aG9kTmFtZSA9ICduZ09uRGVzdHJveScpID0+IDxUPihcbiAgc291cmNlOiBPYnNlcnZhYmxlPFQ+LFxuKSA9PiB7XG4gIGNvbnN0IG9yaWdpbmFsRGVzdHJveSA9IGNvbXBvbmVudEluc3RhbmNlW2Rlc3Ryb3lNZXRob2ROYW1lXTtcbiAgaWYgKGlzRnVuY3Rpb24ob3JpZ2luYWxEZXN0cm95KSA9PT0gZmFsc2UpIHtcbiAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICBgJHtjb21wb25lbnRJbnN0YW5jZS5jb25zdHJ1Y3Rvci5uYW1lfSBpcyB1c2luZyB1bnRpbERlc3Ryb3llZCBidXQgZG9lc24ndCBpbXBsZW1lbnQgJHtkZXN0cm95TWV0aG9kTmFtZX1gLFxuICAgICk7XG4gIH1cbiAgaWYgKCFjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10pIHtcbiAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10gPSBuZXcgU3ViamVjdCgpO1xuXG4gICAgY29tcG9uZW50SW5zdGFuY2VbZGVzdHJveU1ldGhvZE5hbWVdID0gZnVuY3Rpb24oKSB7XG4gICAgICBpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgJiYgb3JpZ2luYWxEZXN0cm95LmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10ubmV4dCh0cnVlKTtcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5jb21wbGV0ZSgpO1xuICAgIH07XG4gIH1cbiAgcmV0dXJuIHNvdXJjZS5waXBlKHRha2VVbnRpbDxUPihjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10pKTtcbn07XG4iXX0=