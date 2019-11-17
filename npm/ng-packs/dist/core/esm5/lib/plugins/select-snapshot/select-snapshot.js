/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { StaticInjector } from './static-injector';
import { META_KEY, getPropsArray, propGetter, removeDollarAtTheEnd } from './internals';
import { Store } from '@ngxs/store';
import { NgxsConfig } from '@ngxs/store/src/symbols';
/**
 * @param {?=} selectorOrFeature
 * @param {...?} paths
 * @return {?}
 */
export function SelectSnapshot(selectorOrFeature) {
    var paths = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        paths[_i - 1] = arguments[_i];
    }
    return (/**
     * @param {?} target
     * @param {?} name
     * @return {?}
     */
    function (target, name) {
        /** @type {?} */
        var selectorFnName = "__" + name + "__selector";
        if (!selectorOrFeature) {
            selectorOrFeature = removeDollarAtTheEnd(name);
        }
        /** @type {?} */
        var createSelector = (/**
         * @param {?} selectorOrFeature
         * @return {?}
         */
        function (selectorOrFeature) {
            /** @type {?} */
            var config = getConfig();
            if (typeof selectorOrFeature === 'string') {
                /** @type {?} */
                var propsArray = getPropsArray(selectorOrFeature, paths);
                return propGetter(propsArray, config);
            }
            else if (selectorOrFeature[META_KEY] && selectorOrFeature[META_KEY].path) {
                return propGetter(selectorOrFeature[META_KEY].path.split('.'), config);
            }
            else {
                return selectorOrFeature;
            }
        });
        if (delete target[name]) {
            Object.defineProperty(target, selectorFnName, {
                writable: true,
                enumerable: false,
                configurable: true,
            });
            Object.defineProperty(target, name, {
                get: (/**
                 * @return {?}
                 */
                function () {
                    // Create anonymous function that will map to the needed state only once
                    /** @type {?} */
                    var selector = this[selectorFnName] || (this[selectorFnName] = createSelector(selectorOrFeature));
                    /** @type {?} */
                    var store = getStore();
                    return store.selectSnapshot(selector);
                }),
                enumerable: true,
                configurable: true,
            });
        }
    });
}
/**
 * @return {?}
 */
function getStore() {
    return StaticInjector.injector.get(Store);
}
/**
 * @return {?}
 */
function getConfig() {
    return StaticInjector.injector.get(NgxsConfig);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2VsZWN0LXNuYXBzaG90LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3BsdWdpbnMvc2VsZWN0LXNuYXBzaG90L3NlbGVjdC1zbmFwc2hvdC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG1CQUFtQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLFVBQVUsRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN4RixPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSx5QkFBeUIsQ0FBQzs7Ozs7O0FBRXJELE1BQU0sVUFBVSxjQUFjLENBQUMsaUJBQXVCO0lBQUUsZUFBa0I7U0FBbEIsVUFBa0IsRUFBbEIscUJBQWtCLEVBQWxCLElBQWtCO1FBQWxCLDhCQUFrQjs7SUFDeEU7Ozs7O0lBQU8sVUFBQyxNQUFXLEVBQUUsSUFBWTs7WUFDekIsY0FBYyxHQUFHLE9BQUssSUFBSSxlQUFZO1FBRTVDLElBQUksQ0FBQyxpQkFBaUIsRUFBRTtZQUN0QixpQkFBaUIsR0FBRyxvQkFBb0IsQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNoRDs7WUFFSyxjQUFjOzs7O1FBQUcsVUFBQyxpQkFBc0I7O2dCQUN0QyxNQUFNLEdBQUcsU0FBUyxFQUFFO1lBRTFCLElBQUksT0FBTyxpQkFBaUIsS0FBSyxRQUFRLEVBQUU7O29CQUNuQyxVQUFVLEdBQUcsYUFBYSxDQUFDLGlCQUFpQixFQUFFLEtBQUssQ0FBQztnQkFDMUQsT0FBTyxVQUFVLENBQUMsVUFBVSxFQUFFLE1BQU0sQ0FBQyxDQUFDO2FBQ3ZDO2lCQUFNLElBQUksaUJBQWlCLENBQUMsUUFBUSxDQUFDLElBQUksaUJBQWlCLENBQUMsUUFBUSxDQUFDLENBQUMsSUFBSSxFQUFFO2dCQUMxRSxPQUFPLFVBQVUsQ0FBQyxpQkFBaUIsQ0FBQyxRQUFRLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxFQUFFLE1BQU0sQ0FBQyxDQUFDO2FBQ3hFO2lCQUFNO2dCQUNMLE9BQU8saUJBQWlCLENBQUM7YUFDMUI7UUFDSCxDQUFDLENBQUE7UUFFRCxJQUFJLE9BQU8sTUFBTSxDQUFDLElBQUksQ0FBQyxFQUFFO1lBQ3ZCLE1BQU0sQ0FBQyxjQUFjLENBQUMsTUFBTSxFQUFFLGNBQWMsRUFBRTtnQkFDNUMsUUFBUSxFQUFFLElBQUk7Z0JBQ2QsVUFBVSxFQUFFLEtBQUs7Z0JBQ2pCLFlBQVksRUFBRSxJQUFJO2FBQ25CLENBQUMsQ0FBQztZQUVILE1BQU0sQ0FBQyxjQUFjLENBQUMsTUFBTSxFQUFFLElBQUksRUFBRTtnQkFDbEMsR0FBRzs7O2dCQUFFOzs7d0JBRUcsUUFBUSxHQUFHLElBQUksQ0FBQyxjQUFjLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxjQUFjLENBQUMsaUJBQWlCLENBQUMsQ0FBQzs7d0JBQzdGLEtBQUssR0FBRyxRQUFRLEVBQUU7b0JBQ3hCLE9BQU8sS0FBSyxDQUFDLGNBQWMsQ0FBQyxRQUFRLENBQUMsQ0FBQztnQkFDeEMsQ0FBQyxDQUFBO2dCQUNELFVBQVUsRUFBRSxJQUFJO2dCQUNoQixZQUFZLEVBQUUsSUFBSTthQUNuQixDQUFDLENBQUM7U0FDSjtJQUNILENBQUMsRUFBQztBQUNKLENBQUM7Ozs7QUFFRCxTQUFTLFFBQVE7SUFDZixPQUFPLGNBQWMsQ0FBQyxRQUFRLENBQUMsR0FBRyxDQUFRLEtBQUssQ0FBQyxDQUFDO0FBQ25ELENBQUM7Ozs7QUFFRCxTQUFTLFNBQVM7SUFDaEIsT0FBTyxjQUFjLENBQUMsUUFBUSxDQUFDLEdBQUcsQ0FBYSxVQUFVLENBQUMsQ0FBQztBQUM3RCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGljSW5qZWN0b3IgfSBmcm9tICcuL3N0YXRpYy1pbmplY3Rvcic7XG5pbXBvcnQgeyBNRVRBX0tFWSwgZ2V0UHJvcHNBcnJheSwgcHJvcEdldHRlciwgcmVtb3ZlRG9sbGFyQXRUaGVFbmQgfSBmcm9tICcuL2ludGVybmFscyc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE5neHNDb25maWcgfSBmcm9tICdAbmd4cy9zdG9yZS9zcmMvc3ltYm9scyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBTZWxlY3RTbmFwc2hvdChzZWxlY3Rvck9yRmVhdHVyZT86IGFueSwgLi4ucGF0aHM6IHN0cmluZ1tdKSB7XG4gIHJldHVybiAodGFyZ2V0OiBhbnksIG5hbWU6IHN0cmluZykgPT4ge1xuICAgIGNvbnN0IHNlbGVjdG9yRm5OYW1lID0gYF9fJHtuYW1lfV9fc2VsZWN0b3JgO1xuXG4gICAgaWYgKCFzZWxlY3Rvck9yRmVhdHVyZSkge1xuICAgICAgc2VsZWN0b3JPckZlYXR1cmUgPSByZW1vdmVEb2xsYXJBdFRoZUVuZChuYW1lKTtcbiAgICB9XG5cbiAgICBjb25zdCBjcmVhdGVTZWxlY3RvciA9IChzZWxlY3Rvck9yRmVhdHVyZTogYW55KSA9PiB7XG4gICAgICBjb25zdCBjb25maWcgPSBnZXRDb25maWcoKTtcblxuICAgICAgaWYgKHR5cGVvZiBzZWxlY3Rvck9yRmVhdHVyZSA9PT0gJ3N0cmluZycpIHtcbiAgICAgICAgY29uc3QgcHJvcHNBcnJheSA9IGdldFByb3BzQXJyYXkoc2VsZWN0b3JPckZlYXR1cmUsIHBhdGhzKTtcbiAgICAgICAgcmV0dXJuIHByb3BHZXR0ZXIocHJvcHNBcnJheSwgY29uZmlnKTtcbiAgICAgIH0gZWxzZSBpZiAoc2VsZWN0b3JPckZlYXR1cmVbTUVUQV9LRVldICYmIHNlbGVjdG9yT3JGZWF0dXJlW01FVEFfS0VZXS5wYXRoKSB7XG4gICAgICAgIHJldHVybiBwcm9wR2V0dGVyKHNlbGVjdG9yT3JGZWF0dXJlW01FVEFfS0VZXS5wYXRoLnNwbGl0KCcuJyksIGNvbmZpZyk7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICByZXR1cm4gc2VsZWN0b3JPckZlYXR1cmU7XG4gICAgICB9XG4gICAgfTtcblxuICAgIGlmIChkZWxldGUgdGFyZ2V0W25hbWVdKSB7XG4gICAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkodGFyZ2V0LCBzZWxlY3RvckZuTmFtZSwge1xuICAgICAgICB3cml0YWJsZTogdHJ1ZSxcbiAgICAgICAgZW51bWVyYWJsZTogZmFsc2UsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZSxcbiAgICAgIH0pO1xuXG4gICAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkodGFyZ2V0LCBuYW1lLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24oKSB7XG4gICAgICAgICAgLy8gQ3JlYXRlIGFub255bW91cyBmdW5jdGlvbiB0aGF0IHdpbGwgbWFwIHRvIHRoZSBuZWVkZWQgc3RhdGUgb25seSBvbmNlXG4gICAgICAgICAgY29uc3Qgc2VsZWN0b3IgPSB0aGlzW3NlbGVjdG9yRm5OYW1lXSB8fCAodGhpc1tzZWxlY3RvckZuTmFtZV0gPSBjcmVhdGVTZWxlY3RvcihzZWxlY3Rvck9yRmVhdHVyZSkpO1xuICAgICAgICAgIGNvbnN0IHN0b3JlID0gZ2V0U3RvcmUoKTtcbiAgICAgICAgICByZXR1cm4gc3RvcmUuc2VsZWN0U25hcHNob3Qoc2VsZWN0b3IpO1xuICAgICAgICB9LFxuICAgICAgICBlbnVtZXJhYmxlOiB0cnVlLFxuICAgICAgICBjb25maWd1cmFibGU6IHRydWUsXG4gICAgICB9KTtcbiAgICB9XG4gIH07XG59XG5cbmZ1bmN0aW9uIGdldFN0b3JlKCk6IFN0b3JlIHtcbiAgcmV0dXJuIFN0YXRpY0luamVjdG9yLmluamVjdG9yLmdldDxTdG9yZT4oU3RvcmUpO1xufVxuXG5mdW5jdGlvbiBnZXRDb25maWcoKTogTmd4c0NvbmZpZyB7XG4gIHJldHVybiBTdGF0aWNJbmplY3Rvci5pbmplY3Rvci5nZXQ8Tmd4c0NvbmZpZz4oTmd4c0NvbmZpZyk7XG59XG4iXX0=