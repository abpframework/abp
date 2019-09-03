/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
/** @type {?} */
var DOLLAR_CHAR_CODE = 36;
/**
 * @param {?} name
 * @return {?}
 */
export function removeDollarAtTheEnd(name) {
    /** @type {?} */
    var lastCharIndex = name.length - 1;
    /** @type {?} */
    var dollarAtTheEnd = name.charCodeAt(lastCharIndex) === DOLLAR_CHAR_CODE;
    return dollarAtTheEnd ? name.slice(0, lastCharIndex) : name;
}
/**
 * @param {?} selectorOrFeature
 * @param {?} paths
 * @return {?}
 */
export function getPropsArray(selectorOrFeature, paths) {
    if (paths.length) {
        return tslib_1.__spread([selectorOrFeature], paths);
    }
    return selectorOrFeature.split('.');
}
/**
 * @param {?} paths
 * @return {?}
 */
function compliantPropGetter(paths) {
    /** @type {?} */
    var copyOfPaths = tslib_1.__spread(paths);
    return (/**
     * @param {?} obj
     * @return {?}
     */
    function (obj) { return copyOfPaths.reduce((/**
     * @param {?} acc
     * @param {?} part
     * @return {?}
     */
    function (acc, part) { return acc && acc[part]; }), obj); });
}
/**
 * @param {?} paths
 * @return {?}
 */
function fastPropGetter(paths) {
    /** @type {?} */
    var segments = paths;
    /** @type {?} */
    var seg = 'store.' + segments[0];
    /** @type {?} */
    var i = 0;
    /** @type {?} */
    var l = segments.length;
    /** @type {?} */
    var expr = seg;
    while (++i < l) {
        expr = expr + ' && ' + (seg = seg + '.' + segments[i]);
    }
    /** @type {?} */
    var fn = new Function('store', 'return ' + expr + ';');
    return (/** @type {?} */ (fn));
}
/**
 * @param {?} paths
 * @param {?} config
 * @return {?}
 */
export function propGetter(paths, config) {
    if (config && config.compatibility && config.compatibility.strictContentSecurityPolicy) {
        return compliantPropGetter(paths);
    }
    else {
        return fastPropGetter(paths);
    }
}
/** @type {?} */
export var META_KEY = 'NGXS_META';
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW50ZXJuYWxzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BsdWdpbnMvc2VsZWN0LXNuYXBzaG90L2ludGVybmFscy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7SUFFTSxnQkFBZ0IsR0FBRyxFQUFFOzs7OztBQUUzQixNQUFNLFVBQVUsb0JBQW9CLENBQUMsSUFBWTs7UUFDekMsYUFBYSxHQUFHLElBQUksQ0FBQyxNQUFNLEdBQUcsQ0FBQzs7UUFDL0IsY0FBYyxHQUFHLElBQUksQ0FBQyxVQUFVLENBQUMsYUFBYSxDQUFDLEtBQUssZ0JBQWdCO0lBQzFFLE9BQU8sY0FBYyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBRSxhQUFhLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDO0FBQzlELENBQUM7Ozs7OztBQUVELE1BQU0sVUFBVSxhQUFhLENBQUMsaUJBQXlCLEVBQUUsS0FBZTtJQUN0RSxJQUFJLEtBQUssQ0FBQyxNQUFNLEVBQUU7UUFDaEIseUJBQVEsaUJBQWlCLEdBQUssS0FBSyxFQUFFO0tBQ3RDO0lBQ0QsT0FBTyxpQkFBaUIsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUM7QUFDdEMsQ0FBQzs7Ozs7QUFFRCxTQUFTLG1CQUFtQixDQUFDLEtBQWU7O1FBQ3BDLFdBQVcsb0JBQU8sS0FBSyxDQUFDO0lBQzlCOzs7O0lBQU8sVUFBQSxHQUFHLElBQUksT0FBQSxXQUFXLENBQUMsTUFBTTs7Ozs7SUFBQyxVQUFDLEdBQVEsRUFBRSxJQUFZLElBQUssT0FBQSxHQUFHLElBQUksR0FBRyxDQUFDLElBQUksQ0FBQyxFQUFoQixDQUFnQixHQUFFLEdBQUcsQ0FBQyxFQUFyRSxDQUFxRSxFQUFDO0FBQ3RGLENBQUM7Ozs7O0FBRUQsU0FBUyxjQUFjLENBQUMsS0FBZTs7UUFDL0IsUUFBUSxHQUFHLEtBQUs7O1FBQ2xCLEdBQUcsR0FBRyxRQUFRLEdBQUcsUUFBUSxDQUFDLENBQUMsQ0FBQzs7UUFDNUIsQ0FBQyxHQUFHLENBQUM7O1FBQ0gsQ0FBQyxHQUFHLFFBQVEsQ0FBQyxNQUFNOztRQUVyQixJQUFJLEdBQUcsR0FBRztJQUNkLE9BQU8sRUFBRSxDQUFDLEdBQUcsQ0FBQyxFQUFFO1FBQ2QsSUFBSSxHQUFHLElBQUksR0FBRyxNQUFNLEdBQUcsQ0FBQyxHQUFHLEdBQUcsR0FBRyxHQUFHLEdBQUcsR0FBRyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztLQUN4RDs7UUFFSyxFQUFFLEdBQUcsSUFBSSxRQUFRLENBQUMsT0FBTyxFQUFFLFNBQVMsR0FBRyxJQUFJLEdBQUcsR0FBRyxDQUFDO0lBRXhELE9BQU8sbUJBQWlCLEVBQUUsRUFBQSxDQUFDO0FBQzdCLENBQUM7Ozs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsS0FBZSxFQUFFLE1BQWtCO0lBQzVELElBQUksTUFBTSxJQUFJLE1BQU0sQ0FBQyxhQUFhLElBQUksTUFBTSxDQUFDLGFBQWEsQ0FBQywyQkFBMkIsRUFBRTtRQUN0RixPQUFPLG1CQUFtQixDQUFDLEtBQUssQ0FBQyxDQUFDO0tBQ25DO1NBQU07UUFDTCxPQUFPLGNBQWMsQ0FBQyxLQUFLLENBQUMsQ0FBQztLQUM5QjtBQUNILENBQUM7O0FBRUQsTUFBTSxLQUFPLFFBQVEsR0FBRyxXQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTmd4c0NvbmZpZyB9IGZyb20gJ0BuZ3hzL3N0b3JlL3NyYy9zeW1ib2xzJztcblxuY29uc3QgRE9MTEFSX0NIQVJfQ09ERSA9IDM2O1xuXG5leHBvcnQgZnVuY3Rpb24gcmVtb3ZlRG9sbGFyQXRUaGVFbmQobmFtZTogc3RyaW5nKTogc3RyaW5nIHtcbiAgY29uc3QgbGFzdENoYXJJbmRleCA9IG5hbWUubGVuZ3RoIC0gMTtcbiAgY29uc3QgZG9sbGFyQXRUaGVFbmQgPSBuYW1lLmNoYXJDb2RlQXQobGFzdENoYXJJbmRleCkgPT09IERPTExBUl9DSEFSX0NPREU7XG4gIHJldHVybiBkb2xsYXJBdFRoZUVuZCA/IG5hbWUuc2xpY2UoMCwgbGFzdENoYXJJbmRleCkgOiBuYW1lO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gZ2V0UHJvcHNBcnJheShzZWxlY3Rvck9yRmVhdHVyZTogc3RyaW5nLCBwYXRoczogc3RyaW5nW10pOiBzdHJpbmdbXSB7XG4gIGlmIChwYXRocy5sZW5ndGgpIHtcbiAgICByZXR1cm4gW3NlbGVjdG9yT3JGZWF0dXJlLCAuLi5wYXRoc107XG4gIH1cbiAgcmV0dXJuIHNlbGVjdG9yT3JGZWF0dXJlLnNwbGl0KCcuJyk7XG59XG5cbmZ1bmN0aW9uIGNvbXBsaWFudFByb3BHZXR0ZXIocGF0aHM6IHN0cmluZ1tdKTogKHg6IGFueSkgPT4gYW55IHtcbiAgY29uc3QgY29weU9mUGF0aHMgPSBbLi4ucGF0aHNdO1xuICByZXR1cm4gb2JqID0+IGNvcHlPZlBhdGhzLnJlZHVjZSgoYWNjOiBhbnksIHBhcnQ6IHN0cmluZykgPT4gYWNjICYmIGFjY1twYXJ0XSwgb2JqKTtcbn1cblxuZnVuY3Rpb24gZmFzdFByb3BHZXR0ZXIocGF0aHM6IHN0cmluZ1tdKTogKHg6IGFueSkgPT4gYW55IHtcbiAgY29uc3Qgc2VnbWVudHMgPSBwYXRocztcbiAgbGV0IHNlZyA9ICdzdG9yZS4nICsgc2VnbWVudHNbMF07XG4gIGxldCBpID0gMDtcbiAgY29uc3QgbCA9IHNlZ21lbnRzLmxlbmd0aDtcblxuICBsZXQgZXhwciA9IHNlZztcbiAgd2hpbGUgKCsraSA8IGwpIHtcbiAgICBleHByID0gZXhwciArICcgJiYgJyArIChzZWcgPSBzZWcgKyAnLicgKyBzZWdtZW50c1tpXSk7XG4gIH1cblxuICBjb25zdCBmbiA9IG5ldyBGdW5jdGlvbignc3RvcmUnLCAncmV0dXJuICcgKyBleHByICsgJzsnKTtcblxuICByZXR1cm4gPCh4OiBhbnkpID0+IGFueT5mbjtcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIHByb3BHZXR0ZXIocGF0aHM6IHN0cmluZ1tdLCBjb25maWc6IE5neHNDb25maWcpIHtcbiAgaWYgKGNvbmZpZyAmJiBjb25maWcuY29tcGF0aWJpbGl0eSAmJiBjb25maWcuY29tcGF0aWJpbGl0eS5zdHJpY3RDb250ZW50U2VjdXJpdHlQb2xpY3kpIHtcbiAgICByZXR1cm4gY29tcGxpYW50UHJvcEdldHRlcihwYXRocyk7XG4gIH0gZWxzZSB7XG4gICAgcmV0dXJuIGZhc3RQcm9wR2V0dGVyKHBhdGhzKTtcbiAgfVxufVxuXG5leHBvcnQgY29uc3QgTUVUQV9LRVkgPSAnTkdYU19NRVRBJztcbiJdfQ==