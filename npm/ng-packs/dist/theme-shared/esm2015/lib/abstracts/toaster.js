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
        this.messageService.add(Object.assign({ severity, detail: message, summary: title }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFDQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7O0FBRzNDLE1BQU0sT0FBTyxlQUFlOzs7O0lBTzFCLFlBQXNCLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUpwRCxRQUFHLEdBQVcsVUFBVSxDQUFDO1FBRXpCLFdBQU0sR0FBWSxLQUFLLENBQUM7SUFFK0IsQ0FBQzs7Ozs7OztJQUN4RCxJQUFJLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQzlDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNwRCxDQUFDOzs7Ozs7O0lBRUQsT0FBTyxDQUFDLE9BQWUsRUFBRSxLQUFhLEVBQUUsT0FBVztRQUNqRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDdkQsQ0FBQzs7Ozs7OztJQUVELElBQUksQ0FBQyxPQUFlLEVBQUUsS0FBYSxFQUFFLE9BQVc7UUFDOUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3BELENBQUM7Ozs7Ozs7SUFFRCxLQUFLLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxPQUFXO1FBQy9DLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE9BQU8sRUFBRSxPQUFPLENBQUMsQ0FBQztJQUNyRCxDQUFDOzs7Ozs7Ozs7SUFFUyxJQUFJLENBQUMsT0FBZSxFQUFFLEtBQWEsRUFBRSxRQUEwQixFQUFFLE9BQVc7UUFDcEYsSUFBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBRXBDLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxpQkFDckIsUUFBUSxFQUNSLE1BQU0sRUFBRSxPQUFPLEVBQ2YsT0FBTyxFQUFFLEtBQUssSUFDWCxPQUFPLElBQ1YsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQ1YsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFPLENBQUMsQ0FBQyxDQUFDLE1BQU0sS0FBSyxXQUFXLElBQUksRUFBRSxNQUFNLEVBQUUsSUFBSSxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQ3RGLENBQUM7UUFDSCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksT0FBTyxFQUFrQixDQUFDO1FBQzdDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELEtBQUssQ0FBQyxNQUF1QjtRQUMzQixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDcEMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsTUFBTSwyQkFBMEIsQ0FBQyxDQUFDO1FBQ3BELElBQUksQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDMUIsQ0FBQztDQUNGOzs7SUEzQ0Msa0NBQWlDOztJQUVqQyw4QkFBeUI7O0lBRXpCLGlDQUF3Qjs7Ozs7SUFFWix5Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBNZXNzYWdlU2VydmljZSB9IGZyb20gJ3ByaW1lbmcvY29tcG9uZW50cy9jb21tb24vbWVzc2FnZXNlcnZpY2UnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcblxuZXhwb3J0IGNsYXNzIEFic3RyYWN0VG9hc3RlcjxUID0gVG9hc3Rlci5PcHRpb25zPiB7XG4gIHN0YXR1cyQ6IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+O1xuXG4gIGtleTogc3RyaW5nID0gJ2FicFRvYXN0JztcblxuICBzdGlja3k6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7fVxuICBpbmZvKG1lc3NhZ2U6IHN0cmluZywgdGl0bGU6IHN0cmluZywgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2luZm8nLCBvcHRpb25zKTtcbiAgfVxuXG4gIHN1Y2Nlc3MobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnc3VjY2VzcycsIG9wdGlvbnMpO1xuICB9XG5cbiAgd2FybihtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICd3YXJuJywgb3B0aW9ucyk7XG4gIH1cblxuICBlcnJvcihtZXNzYWdlOiBzdHJpbmcsIHRpdGxlOiBzdHJpbmcsIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdlcnJvcicsIG9wdGlvbnMpO1xuICB9XG5cbiAgcHJvdGVjdGVkIHNob3cobWVzc2FnZTogc3RyaW5nLCB0aXRsZTogc3RyaW5nLCBzZXZlcml0eTogVG9hc3Rlci5TZXZlcml0eSwgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgdGhpcy5tZXNzYWdlU2VydmljZS5jbGVhcih0aGlzLmtleSk7XG5cbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZCh7XG4gICAgICBzZXZlcml0eSxcbiAgICAgIGRldGFpbDogbWVzc2FnZSxcbiAgICAgIHN1bW1hcnk6IHRpdGxlLFxuICAgICAgLi4ub3B0aW9ucyxcbiAgICAgIGtleTogdGhpcy5rZXksXG4gICAgICAuLi4odHlwZW9mIChvcHRpb25zIHx8ICh7fSBhcyBhbnkpKS5zdGlja3kgPT09ICd1bmRlZmluZWQnICYmIHsgc3RpY2t5OiB0aGlzLnN0aWNreSB9KSxcbiAgICB9KTtcbiAgICB0aGlzLnN0YXR1cyQgPSBuZXcgU3ViamVjdDxUb2FzdGVyLlN0YXR1cz4oKTtcbiAgICByZXR1cm4gdGhpcy5zdGF0dXMkO1xuICB9XG5cbiAgY2xlYXIoc3RhdHVzPzogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcbiAgICB0aGlzLnN0YXR1cyQubmV4dChzdGF0dXMgfHwgVG9hc3Rlci5TdGF0dXMuZGlzbWlzcyk7XG4gICAgdGhpcy5zdGF0dXMkLmNvbXBsZXRlKCk7XG4gIH1cbn1cbiJdfQ==