/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import { fromEvent, Subject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
var ConfirmationService = /** @class */ (function (_super) {
    tslib_1.__extends(ConfirmationService, _super);
    function ConfirmationService(messageService) {
        var _this = _super.call(this, messageService) || this;
        _this.messageService = messageService;
        _this.key = 'abpConfirmation';
        _this.sticky = true;
        _this.destroy$ = new Subject();
        return _this;
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    ConfirmationService.prototype.show = /**
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    function (message, title, severity, options) {
        this.listenToEscape();
        return _super.prototype.show.call(this, message, title, severity, options);
    };
    /**
     * @param {?=} status
     * @return {?}
     */
    ConfirmationService.prototype.clear = /**
     * @param {?=} status
     * @return {?}
     */
    function (status) {
        _super.prototype.clear.call(this, status);
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    ConfirmationService.prototype.listenToEscape = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.code === 'Escape'; })))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.clear();
        }));
    };
    ConfirmationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    ConfirmationService.ctorParameters = function () { return [
        { type: MessageService }
    ]; };
    /** @nocollapse */ ConfirmationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(i0.ɵɵinject(i1.MessageService)); }, token: ConfirmationService, providedIn: "root" });
    return ConfirmationService;
}(AbstractToaster));
export { ConfirmationService };
if (false) {
    /** @type {?} */
    ConfirmationService.prototype.key;
    /** @type {?} */
    ConfirmationService.prototype.sticky;
    /** @type {?} */
    ConfirmationService.prototype.destroy$;
    /**
     * @type {?}
     * @protected
     */
    ConfirmationService.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBRXZELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsU0FBUyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7O0FBR2pFO0lBQ3lDLCtDQUFxQztJQU81RSw2QkFBc0IsY0FBOEI7UUFBcEQsWUFDRSxrQkFBTSxjQUFjLENBQUMsU0FDdEI7UUFGcUIsb0JBQWMsR0FBZCxjQUFjLENBQWdCO1FBTnBELFNBQUcsR0FBRyxpQkFBaUIsQ0FBQztRQUV4QixZQUFNLEdBQUcsSUFBSSxDQUFDO1FBRWQsY0FBUSxHQUFHLElBQUksT0FBTyxFQUFFLENBQUM7O0lBSXpCLENBQUM7Ozs7Ozs7O0lBRUQsa0NBQUk7Ozs7Ozs7SUFBSixVQUNFLE9BQWUsRUFDZixLQUFhLEVBQ2IsUUFBMEIsRUFDMUIsT0FBOEI7UUFFOUIsSUFBSSxDQUFDLGNBQWMsRUFBRSxDQUFDO1FBRXRCLE9BQU8saUJBQU0sSUFBSSxZQUFDLE9BQU8sRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLE9BQU8sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7O0lBRUQsbUNBQUs7Ozs7SUFBTCxVQUFNLE1BQXVCO1FBQzNCLGlCQUFNLEtBQUssWUFBQyxNQUFNLENBQUMsQ0FBQztRQUVwQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ3ZCLENBQUM7Ozs7SUFFRCw0Q0FBYzs7O0lBQWQ7UUFBQSxpQkFVQztRQVRDLFNBQVMsQ0FBQyxRQUFRLEVBQUUsT0FBTyxDQUFDO2FBQ3pCLElBQUksQ0FDSCxTQUFTLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUN4QixZQUFZLENBQUMsR0FBRyxDQUFDLEVBQ2pCLE1BQU07Ozs7UUFBQyxVQUFDLEdBQWtCLElBQUssT0FBQSxHQUFHLElBQUksR0FBRyxDQUFDLElBQUksS0FBSyxRQUFRLEVBQTVCLENBQTRCLEVBQUMsQ0FDN0Q7YUFDQSxTQUFTOzs7O1FBQUMsVUFBQSxDQUFDO1lBQ1YsS0FBSSxDQUFDLEtBQUssRUFBRSxDQUFDO1FBQ2YsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOztnQkF2Q0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztnQkFMekIsY0FBYzs7OzhCQUh2QjtDQWdEQyxBQXhDRCxDQUN5QyxlQUFlLEdBdUN2RDtTQXZDWSxtQkFBbUI7OztJQUM5QixrQ0FBd0I7O0lBRXhCLHFDQUFjOztJQUVkLHVDQUF5Qjs7Ozs7SUFFYiw2Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFic3RyYWN0VG9hc3RlciB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcclxuaW1wb3J0IHsgQ29uZmlybWF0aW9uIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpcm1hdGlvbic7XHJcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XHJcbmltcG9ydCB7IGZyb21FdmVudCwgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyB0YWtlVW50aWwsIGRlYm91bmNlVGltZSwgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vbW9kZWxzL3RvYXN0ZXInO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvblNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFRvYXN0ZXI8Q29uZmlybWF0aW9uLk9wdGlvbnM+IHtcclxuICBrZXkgPSAnYWJwQ29uZmlybWF0aW9uJztcclxuXHJcbiAgc3RpY2t5ID0gdHJ1ZTtcclxuXHJcbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdCgpO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7XHJcbiAgICBzdXBlcihtZXNzYWdlU2VydmljZSk7XHJcbiAgfVxyXG5cclxuICBzaG93KFxyXG4gICAgbWVzc2FnZTogc3RyaW5nLFxyXG4gICAgdGl0bGU6IHN0cmluZyxcclxuICAgIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LFxyXG4gICAgb3B0aW9ucz86IENvbmZpcm1hdGlvbi5PcHRpb25zXHJcbiAgKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xyXG4gICAgdGhpcy5saXN0ZW5Ub0VzY2FwZSgpO1xyXG5cclxuICAgIHJldHVybiBzdXBlci5zaG93KG1lc3NhZ2UsIHRpdGxlLCBzZXZlcml0eSwgb3B0aW9ucyk7XHJcbiAgfVxyXG5cclxuICBjbGVhcihzdGF0dXM/OiBUb2FzdGVyLlN0YXR1cykge1xyXG4gICAgc3VwZXIuY2xlYXIoc3RhdHVzKTtcclxuXHJcbiAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcclxuICB9XHJcblxyXG4gIGxpc3RlblRvRXNjYXBlKCkge1xyXG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAna2V5dXAnKVxyXG4gICAgICAucGlwZShcclxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXHJcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXHJcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkuY29kZSA9PT0gJ0VzY2FwZScpXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZShfID0+IHtcclxuICAgICAgICB0aGlzLmNsZWFyKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=