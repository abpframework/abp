/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
/**
 * @template T
 */
export class AbstractToaster {
    /**
     * @param {?} messageService
     */
    constructor(messageService) {
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
    info(message, title, options) {
        return this.show(message, title, 'info', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    success(message, title, options) {
        return this.show(message, title, 'success', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    warn(message, title, options) {
        return this.show(message, title, 'warn', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    error(message, title, options) {
        return this.show(message, title, 'error', options);
    }
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    show(message, title, severity, options) {
        this.messageService.clear(this.key);
        this.messageService.add(Object.assign({ severity, detail: message || '', summary: title || '' }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
        this.status$ = new Subject();
        return this.status$;
    }
    /**
     * @param {?=} status
     * @return {?}
     */
    clear(status) {
        this.messageService.clear(this.key);
        this.status$.next(status || "dismiss" /* dismiss */);
        this.status$.complete();
    }
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFDQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7O0FBRzNDLE1BQU0sT0FBTyxlQUFlOzs7O0lBTzFCLFlBQXNCLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUpwRCxRQUFHLEdBQVcsVUFBVSxDQUFDO1FBRXpCLFdBQU0sR0FBWSxLQUFLLENBQUM7SUFFK0IsQ0FBQzs7Ozs7OztJQUN4RCxJQUFJLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQzlDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNwRCxDQUFDOzs7Ozs7O0lBRUQsT0FBTyxDQUFDLE9BQWUsRUFBRSxLQUFhLEVBQUUsT0FBVztRQUNqRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDdkQsQ0FBQzs7Ozs7OztJQUVELElBQUksQ0FBQyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxLQUFLLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQy9DLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE9BQU8sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNyRCxDQUFDOzs7Ozs7Ozs7SUFFUyxJQUFJLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxRQUEwQixFQUFFLE9BQVc7UUFDcEYsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBRXBDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxpQkFDckIsUUFBUSxFQUNSLE1BQU0sRUFBRSxPQUFPLElBQUksRUFBRSxFQUNyQixPQUFPLEVBQUUsS0FBSyxJQUFJLEVBQUUsSUFDakIsT0FBTyxJQUNWLEdBQUcsRUFBRSxJQUFJLENBQUMsR0FBRyxJQUNWLENBQUMsT0FBTyxDQUFDLE9BQU8sSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBTyxDQUFDLENBQUMsQ0FBQyxNQUFNLEtBQUssV0FBVyxJQUFJLEVBQUUsTUFBTSxFQUFFLElBQUksQ0FBQyxNQUFNLEVBQUUsQ0FBQyxFQUN0RixDQUFDO1FBQ0gsSUFBSSxDQUFDLE9BQU8sR0FBRyxJQUFJLE9BQU8sRUFBa0IsQ0FBQztRQUM3QyxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUM7SUFDdEIsQ0FBQzs7Ozs7SUFFRCxLQUFLLENBQUMsTUFBdUI7UUFDM0IsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQ3BDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLE1BQU0sMkJBQTBCLENBQUMsQ0FBQztRQUNwRCxJQUFJLENBQUMsT0FBTyxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQzFCLENBQUM7Q0FDRjs7O0lBM0NDLGtDQUFpQzs7SUFFakMsOEJBQXlCOztJQUV6QixpQ0FBd0I7Ozs7O0lBRVoseUNBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTWVzc2FnZVNlcnZpY2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2VzZXJ2aWNlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XG5cbmV4cG9ydCBjbGFzcyBBYnN0cmFjdFRvYXN0ZXI8VCA9IFRvYXN0ZXIuT3B0aW9ucz4ge1xuICBzdGF0dXMkOiBTdWJqZWN0PFRvYXN0ZXIuU3RhdHVzPjtcblxuICBrZXk6IHN0cmluZyA9ICdhYnBUb2FzdCc7XG5cbiAgc3RpY2t5OiBib29sZWFuID0gZmFsc2U7XG5cbiAgY29uc3RydWN0b3IocHJvdGVjdGVkIG1lc3NhZ2VTZXJ2aWNlOiBNZXNzYWdlU2VydmljZSkge31cbiAgaW5mbyhtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdpbmZvJywgb3B0aW9ucyk7XG4gIH1cblxuICBzdWNjZXNzKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ3N1Y2Nlc3MnLCBvcHRpb25zKTtcbiAgfVxuXG4gIHdhcm4obWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnd2FybicsIG9wdGlvbnMpO1xuICB9XG5cbiAgZXJyb3IobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnZXJyb3InLCBvcHRpb25zKTtcbiAgfVxuXG4gIHByb3RlY3RlZCBzaG93KG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgc2V2ZXJpdHk6IFRvYXN0ZXIuU2V2ZXJpdHksIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuY2xlYXIodGhpcy5rZXkpO1xuXG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5hZGQoe1xuICAgICAgc2V2ZXJpdHksXG4gICAgICBkZXRhaWw6IG1lc3NhZ2UgfHwgJycsXG4gICAgICBzdW1tYXJ5OiB0aXRsZSB8fCAnJyxcbiAgICAgIC4uLm9wdGlvbnMsXG4gICAgICBrZXk6IHRoaXMua2V5LFxuICAgICAgLi4uKHR5cGVvZiAob3B0aW9ucyB8fCAoe30gYXMgYW55KSkuc3RpY2t5ID09PSAndW5kZWZpbmVkJyAmJiB7IHN0aWNreTogdGhpcy5zdGlja3kgfSksXG4gICAgfSk7XG4gICAgdGhpcy5zdGF0dXMkID0gbmV3IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+KCk7XG4gICAgcmV0dXJuIHRoaXMuc3RhdHVzJDtcbiAgfVxuXG4gIGNsZWFyKHN0YXR1cz86IFRvYXN0ZXIuU3RhdHVzKSB7XG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XG4gICAgdGhpcy5zdGF0dXMkLm5leHQoc3RhdHVzIHx8IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3MpO1xuICAgIHRoaXMuc3RhdHVzJC5jb21wbGV0ZSgpO1xuICB9XG59XG4iXX0=