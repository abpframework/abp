/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
/**
 * @abstract
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy90b2FzdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFDQSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDOzs7OztBQUkzQyxNQUFNLE9BQWdCLGVBQWU7Ozs7SUFPbkMsWUFBc0IsY0FBOEI7UUFBOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBSnBELFFBQUcsR0FBRyxVQUFVLENBQUM7UUFFakIsV0FBTSxHQUFHLEtBQUssQ0FBQztJQUV3QyxDQUFDOzs7Ozs7O0lBRXhELElBQUksQ0FBQyxPQUFpQyxFQUFFLEtBQStCLEVBQUUsT0FBVztRQUNsRixPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7Ozs7OztJQUVELE9BQU8sQ0FBQyxPQUFpQyxFQUFFLEtBQStCLEVBQUUsT0FBVztRQUNyRixPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDdkQsQ0FBQzs7Ozs7OztJQUVELElBQUksQ0FBQyxPQUFpQyxFQUFFLEtBQStCLEVBQUUsT0FBVztRQUNsRixPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7Ozs7OztJQUVELEtBQUssQ0FBQyxPQUFpQyxFQUFFLEtBQStCLEVBQUUsT0FBVztRQUNuRixPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxPQUFPLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDckQsQ0FBQzs7Ozs7Ozs7O0lBRVMsSUFBSSxDQUNaLE9BQWlDLEVBQ2pDLEtBQStCLEVBQy9CLFFBQTBCLEVBQzFCLE9BQVc7UUFFWCxJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFFcEMsSUFBSSxDQUFDLGNBQWMsQ0FBQyxHQUFHLGlCQUNyQixRQUFRLEVBQ1IsTUFBTSxFQUFFLE9BQU8sSUFBSSxFQUFFLEVBQ3JCLE9BQU8sRUFBRSxLQUFLLElBQUksRUFBRSxJQUNqQixPQUFPLElBQ1YsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQ1YsQ0FBQyxPQUFPLENBQUMsT0FBTyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFPLENBQUMsQ0FBQyxDQUFDLE1BQU0sS0FBSyxXQUFXLElBQUksRUFBRSxNQUFNLEVBQUUsSUFBSSxDQUFDLE1BQU0sRUFBRSxDQUFDLEVBQ3RGLENBQUM7UUFDSCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksT0FBTyxFQUFrQixDQUFDO1FBQzdDLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7OztJQUVELEtBQUssQ0FBQyxNQUF1QjtRQUMzQixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDcEMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsTUFBTSwyQkFBMEIsQ0FBQyxDQUFDO1FBQ3BELElBQUksQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDMUIsQ0FBQztDQUNGOzs7SUFqREMsa0NBQWlDOztJQUVqQyw4QkFBaUI7O0lBRWpCLGlDQUFlOzs7OztJQUVILHlDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcclxuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbmV4cG9ydCBhYnN0cmFjdCBjbGFzcyBBYnN0cmFjdFRvYXN0ZXI8VCA9IFRvYXN0ZXIuT3B0aW9ucz4ge1xyXG4gIHN0YXR1cyQ6IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+O1xyXG5cclxuICBrZXkgPSAnYWJwVG9hc3QnO1xyXG5cclxuICBzdGlja3kgPSBmYWxzZTtcclxuXHJcbiAgY29uc3RydWN0b3IocHJvdGVjdGVkIG1lc3NhZ2VTZXJ2aWNlOiBNZXNzYWdlU2VydmljZSkge31cclxuXHJcbiAgaW5mbyhtZXNzYWdlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sIHRpdGxlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sIG9wdGlvbnM/OiBUKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xyXG4gICAgcmV0dXJuIHRoaXMuc2hvdyhtZXNzYWdlLCB0aXRsZSwgJ2luZm8nLCBvcHRpb25zKTtcclxuICB9XHJcblxyXG4gIHN1Y2Nlc3MobWVzc2FnZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcclxuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdzdWNjZXNzJywgb3B0aW9ucyk7XHJcbiAgfVxyXG5cclxuICB3YXJuKG1lc3NhZ2U6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSwgdGl0bGU6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSwgb3B0aW9ucz86IFQpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XHJcbiAgICByZXR1cm4gdGhpcy5zaG93KG1lc3NhZ2UsIHRpdGxlLCAnd2FybicsIG9wdGlvbnMpO1xyXG4gIH1cclxuXHJcbiAgZXJyb3IobWVzc2FnZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLCBvcHRpb25zPzogVCk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcclxuICAgIHJldHVybiB0aGlzLnNob3cobWVzc2FnZSwgdGl0bGUsICdlcnJvcicsIG9wdGlvbnMpO1xyXG4gIH1cclxuXHJcbiAgcHJvdGVjdGVkIHNob3coXHJcbiAgICBtZXNzYWdlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sXHJcbiAgICB0aXRsZTogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLFxyXG4gICAgc2V2ZXJpdHk6IFRvYXN0ZXIuU2V2ZXJpdHksXHJcbiAgICBvcHRpb25zPzogVCxcclxuICApOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XHJcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmNsZWFyKHRoaXMua2V5KTtcclxuXHJcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZCh7XHJcbiAgICAgIHNldmVyaXR5LFxyXG4gICAgICBkZXRhaWw6IG1lc3NhZ2UgfHwgJycsXHJcbiAgICAgIHN1bW1hcnk6IHRpdGxlIHx8ICcnLFxyXG4gICAgICAuLi5vcHRpb25zLFxyXG4gICAgICBrZXk6IHRoaXMua2V5LFxyXG4gICAgICAuLi4odHlwZW9mIChvcHRpb25zIHx8ICh7fSBhcyBhbnkpKS5zdGlja3kgPT09ICd1bmRlZmluZWQnICYmIHsgc3RpY2t5OiB0aGlzLnN0aWNreSB9KSxcclxuICAgIH0pO1xyXG4gICAgdGhpcy5zdGF0dXMkID0gbmV3IFN1YmplY3Q8VG9hc3Rlci5TdGF0dXM+KCk7XHJcbiAgICByZXR1cm4gdGhpcy5zdGF0dXMkO1xyXG4gIH1cclxuXHJcbiAgY2xlYXIoc3RhdHVzPzogVG9hc3Rlci5TdGF0dXMpIHtcclxuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuY2xlYXIodGhpcy5rZXkpO1xyXG4gICAgdGhpcy5zdGF0dXMkLm5leHQoc3RhdHVzIHx8IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3MpO1xyXG4gICAgdGhpcy5zdGF0dXMkLmNvbXBsZXRlKCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==