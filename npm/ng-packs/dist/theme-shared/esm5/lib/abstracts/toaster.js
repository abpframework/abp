/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Subject } from 'rxjs';
/**
 * @abstract
 * @template T
 */
var /**
 * @abstract
 * @template T
 */
AbstractToaster = /** @class */ (function () {
    function AbstractToaster(messageService) {
        this.messageService = messageService;
        this.key = 'abpToast';
        this.sticky = false;
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.info = /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    function (message, title, options) {
        return this.show(message, title, 'info', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.success = /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    function (message, title, options) {
        return this.show(message, title, 'success', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.warn = /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    function (message, title, options) {
        return this.show(message, title, 'warn', options);
    };
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.error = /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    function (message, title, options) {
        return this.show(message, title, 'error', options);
    };
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    AbstractToaster.prototype.show = /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    function (message, title, severity, options) {
        this.messageService.clear(this.key);
        this.messageService.add(tslib_1.__assign({ severity: severity, detail: message || '', summary: title || '' }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
        this.status$ = new Subject();
        return this.status$;
    };
    /**
     * @param {?=} status
     * @return {?}
     */
    AbstractToaster.prototype.clear = /**
     * @param {?=} status
     * @return {?}
     */
    function (status) {
        this.messageService.clear(this.key);
        this.status$.next(status || "dismiss" /* dismiss */);
        this.status$.complete();
    };
    return AbstractToaster;
}());
/**
 * @abstract
 * @template T
 */
export { AbstractToaster };
if (false) {
    /** @type {?} */
    AbstractToaster.prototype.status$;
    /** @type {?} */
    AbstractToaster.prototype.key;
    /** @type {?} */
    AbstractToaster.prototype.sticky;
    /**
     * @type {?}
     * @protected
     */
    AbstractToaster.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQ0EsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQzs7Ozs7QUFHM0M7Ozs7O0lBT0UseUJBQXNCLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUpwRCxRQUFHLEdBQUcsVUFBVSxDQUFDO1FBRWpCLFdBQU0sR0FBRyxLQUFLLENBQUM7SUFFd0MsQ0FBQzs7Ozs7OztJQUV4RCw4QkFBSTs7Ozs7O0lBQUosVUFBSyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxpQ0FBTzs7Ozs7O0lBQVAsVUFBUSxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDakQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7Ozs7SUFFRCw4QkFBSTs7Ozs7O0lBQUosVUFBSyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCwrQkFBSzs7Ozs7O0lBQUwsVUFBTSxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDL0MsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3JELENBQUM7Ozs7Ozs7OztJQUVTLDhCQUFJOzs7Ozs7OztJQUFkLFVBQWUsT0FBZSxFQUFFLEtBQWEsRUFBRSxRQUEwQixFQUFFLE9BQVc7UUFDcEYsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBRXBDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxvQkFDckIsUUFBUSxVQUFBLEVBQ1IsTUFBTSxFQUFFLE9BQU8sSUFBSSxFQUFFLEVBQ3JCLE9BQU8sRUFBRSxLQUFLLElBQUksRUFBRSxJQUNqQixPQUFPLElBQ1YsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQ1YsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFPLENBQUMsQ0FBQyxDQUFDLE1BQU0sS0FBSyxXQUFXLElBQUksRUFBRSxNQUFNLEVBQUUsSUFBSSxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQ3RGLENBQUM7UUFDSCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksT0FBTyxFQUFrQixDQUFDO1FBQzdDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELCtCQUFLOzs7O0lBQUwsVUFBTSxNQUF1QjtRQUMzQixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDcEMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsTUFBTSwyQkFBMEIsQ0FBQyxDQUFDO1FBQ3BELElBQUksQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDMUIsQ0FBQztJQUNILHNCQUFDO0FBQUQsQ0FBQyxBQTdDRCxJQTZDQzs7Ozs7Ozs7SUE1Q0Msa0NBQWlDOztJQUVqQyw4QkFBaUI7O0lBRWpCLGlDQUFlOzs7OztJQUVILHlDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcclxuXHJcbmV4cG9ydCBhYnN0cmFjdCBjbGFzcyBBYnN0cmFjdFRvYXN0ZXI8VCA9IFRvYXN0ZXIuT3B0aW9ucz4ge1xyXG4gIHN0YXR1cyQ6IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+O1xyXG5cclxuICBrZXkgPSAnYWJwVG9hc3QnO1xyXG5cclxuICBzdGlja3kgPSBmYWxzZTtcclxuXHJcbiAgY29uc3RydWN0b3IocHJvdGVjdGVkIG1lc3NhZ2VTZXJ2aWNlOiBNZXNzYWdlU2VydmljZSkge31cclxuXHJcbiAgaW5mbyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xyXG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2luZm8nLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHN1Y2Nlc3MobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcclxuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdzdWNjZXNzJywgb3B0aW9ucyk7XHJcbiAgfVxyXG5cclxuICB3YXJuKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XHJcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnd2FybicsIG9wdGlvbnMpO1xyXG4gIH1cclxuXHJcbiAgZXJyb3IobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcclxuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdlcnJvcicsIG9wdGlvbnMpO1xyXG4gIH1cclxuXHJcbiAgcHJvdGVjdGVkIHNob3cobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBzZXZlcml0eTogVG9hc3Rlci5TZXZlcml0eSwgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XHJcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcclxuXHJcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZCh7XHJcbiAgICAgIHNldmVyaXR5LFxyXG4gICAgICBkZXRhaWw6IG1lc3NhZ2UgfHwgJycsXHJcbiAgICAgIHN1bW1hcnk6IHRpdGxlIHx8ICcnLFxyXG4gICAgICAuLi5vcHRpb25zLFxyXG4gICAgICBrZXk6IHRoaXMua2V5LFxyXG4gICAgICAuLi4odHlwZW9mIChvcHRpb25zIHx8ICh7fSBhcyBhbnkpKS5zdGlja3kgPT09ICd1bmRlZmluZWQnICYmIHsgc3RpY2t5OiB0aGlzLnN0aWNreSB9KVxyXG4gICAgfSk7XHJcbiAgICB0aGlzLnN0YXR1cyQgPSBuZXcgU3ViamVjdDxUb2FzdGVyLlN0YXR1cz4oKTtcclxuICAgIHJldHVybiB0aGlzLnN0YXR1cyQ7XHJcbiAgfVxyXG5cclxuICBjbGVhcihzdGF0dXM/OiBUb2FzdGVyLlN0YXR1cykge1xyXG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XHJcbiAgICB0aGlzLnN0YXR1cyQubmV4dChzdGF0dXMgfHwgVG9hc3Rlci5TdGF0dXMuZGlzbWlzcyk7XHJcbiAgICB0aGlzLnN0YXR1cyQuY29tcGxldGUoKTtcclxuICB9XHJcbn1cclxuIl19