/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Subject } from 'rxjs';
/**
 * @template T
 */
var /**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQ0EsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQzs7OztBQUczQzs7OztJQU9FLHlCQUFzQixjQUE4QjtRQUE5QixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFKcEQsUUFBRyxHQUFXLFVBQVUsQ0FBQztRQUV6QixXQUFNLEdBQVksS0FBSyxDQUFDO0lBRStCLENBQUM7Ozs7Ozs7SUFDeEQsOEJBQUk7Ozs7OztJQUFKLFVBQUssT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQzlDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNwRCxDQUFDOzs7Ozs7O0lBRUQsaUNBQU87Ozs7OztJQUFQLFVBQVEsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQ2pELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLFNBQVMsRUFBRSxPQUFPLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7Ozs7O0lBRUQsOEJBQUk7Ozs7OztJQUFKLFVBQUssT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQzlDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNwRCxDQUFDOzs7Ozs7O0lBRUQsK0JBQUs7Ozs7OztJQUFMLFVBQU0sT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQy9DLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE9BQU8sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNyRCxDQUFDOzs7Ozs7Ozs7SUFFUyw4QkFBSTs7Ozs7Ozs7SUFBZCxVQUFlLE9BQWUsRUFBRSxLQUFhLEVBQUUsUUFBMEIsRUFBRSxPQUFXO1FBQ3BGLElBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUVwQyxJQUFJLENBQUMsY0FBYyxDQUFDLEdBQUcsb0JBQ3JCLFFBQVEsVUFBQSxFQUNSLE1BQU0sRUFBRSxPQUFPLElBQUksRUFBRSxFQUNyQixPQUFPLEVBQUUsS0FBSyxJQUFJLEVBQUUsSUFDakIsT0FBTyxJQUNWLEdBQUcsRUFBRSxJQUFJLENBQUMsR0FBRyxJQUNWLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBTyxDQUFDLENBQUMsQ0FBQyxNQUFNLEtBQUssV0FBVyxJQUFJLEVBQUUsTUFBTSxFQUFFLElBQUksQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUN0RixDQUFDO1FBQ0gsSUFBSSxDQUFDLE9BQU8sR0FBRyxJQUFJLE9BQU8sRUFBa0IsQ0FBQztRQUM3QyxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUM7SUFDdEIsQ0FBQzs7Ozs7SUFFRCwrQkFBSzs7OztJQUFMLFVBQU0sTUFBdUI7UUFDM0IsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQ3BDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLE1BQU0sMkJBQTBCLENBQUMsQ0FBQztRQUNwRCxJQUFJLENBQUMsT0FBTyxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQzFCLENBQUM7SUFDSCxzQkFBQztBQUFELENBQUMsQUE1Q0QsSUE0Q0M7Ozs7Ozs7SUEzQ0Msa0NBQWlDOztJQUVqQyw4QkFBeUI7O0lBRXpCLGlDQUF3Qjs7Ozs7SUFFWix5Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcblxuZXhwb3J0IGNsYXNzIEFic3RyYWN0VG9hc3RlcjxUID0gVG9hc3Rlci5PcHRpb25zPiB7XG4gIHN0YXR1cyQ6IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+O1xuXG4gIGtleTogc3RyaW5nID0gJ2FicFRvYXN0JztcblxuICBzdGlja3k6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7fVxuICBpbmZvKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2luZm8nLCBvcHRpb25zKTtcbiAgfVxuXG4gIHN1Y2Nlc3MobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnc3VjY2VzcycsIG9wdGlvbnMpO1xuICB9XG5cbiAgd2FybihtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICd3YXJuJywgb3B0aW9ucyk7XG4gIH1cblxuICBlcnJvcihtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdlcnJvcicsIG9wdGlvbnMpO1xuICB9XG5cbiAgcHJvdGVjdGVkIHNob3cobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBzZXZlcml0eTogVG9hc3Rlci5TZXZlcml0eSwgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XG5cbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZCh7XG4gICAgICBzZXZlcml0eSxcbiAgICAgIGRldGFpbDogbWVzc2FnZSB8fCAnJyxcbiAgICAgIHN1bW1hcnk6IHRpdGxlIHx8ICcnLFxuICAgICAgLi4ub3B0aW9ucyxcbiAgICAgIGtleTogdGhpcy5rZXksXG4gICAgICAuLi4odHlwZW9mIChvcHRpb25zIHx8ICh7fSBhcyBhbnkpKS5zdGlja3kgPT09ICd1bmRlZmluZWQnICYmIHsgc3RpY2t5OiB0aGlzLnN0aWNreSB9KSxcbiAgICB9KTtcbiAgICB0aGlzLnN0YXR1cyQgPSBuZXcgU3ViamVjdDxUb2FzdGVyLlN0YXR1cz4oKTtcbiAgICByZXR1cm4gdGhpcy5zdGF0dXMkO1xuICB9XG5cbiAgY2xlYXIoc3RhdHVzPzogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcbiAgICB0aGlzLnN0YXR1cyQubmV4dChzdGF0dXMgfHwgVG9hc3Rlci5TdGF0dXMuZGlzbWlzcyk7XG4gICAgdGhpcy5zdGF0dXMkLmNvbXBsZXRlKCk7XG4gIH1cbn1cbiJdfQ==