/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/confirmation.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import { fromEvent, Subject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
export class ConfirmationService extends AbstractToaster {
    /**
     * @param {?} messageService
     */
    constructor(messageService) {
        super(messageService);
        this.messageService = messageService;
        this.key = 'abpConfirmation';
        this.sticky = true;
        this.destroy$ = new Subject();
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    show(message, title, severity, options) {
        this.listenToEscape();
        return super.show(message, title, severity, options);
    }
    /**
     * @param {?=} status
     * @return {?}
     */
    clear(status) {
        super.clear(status);
        this.destroy$.next();
    }
    /**
     * @return {?}
     */
    listenToEscape() {
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.key === 'Escape')))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        _ => {
            this.clear();
        }));
    }
}
ConfirmationService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
ConfirmationService.ctorParameters = () => [
    { type: MessageService }
];
/** @nocollapse */ ConfirmationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(i0.ɵɵinject(i1.MessageService)); }, token: ConfirmationService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBRXZELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsU0FBUyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7O0FBSWpFLE1BQU0sT0FBTyxtQkFBb0IsU0FBUSxlQUFxQzs7OztJQU81RSxZQUFzQixjQUE4QjtRQUNsRCxLQUFLLENBQUMsY0FBYyxDQUFDLENBQUM7UUFERixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFOcEQsUUFBRyxHQUFHLGlCQUFpQixDQUFDO1FBRXhCLFdBQU0sR0FBRyxJQUFJLENBQUM7UUFFZCxhQUFRLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztJQUl6QixDQUFDOzs7Ozs7OztJQUVELElBQUksQ0FDRixPQUFlLEVBQ2YsS0FBYSxFQUNiLFFBQTBCLEVBQzFCLE9BQThCO1FBRTlCLElBQUksQ0FBQyxjQUFjLEVBQUUsQ0FBQztRQUN0QixPQUFPLEtBQUssQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEtBQUssRUFBRSxRQUFRLEVBQUUsT0FBTyxDQUFDLENBQUM7SUFDdkQsQ0FBQzs7Ozs7SUFFRCxLQUFLLENBQUMsTUFBdUI7UUFDM0IsS0FBSyxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsQ0FBQztRQUVwQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ3ZCLENBQUM7Ozs7SUFFRCxjQUFjO1FBQ1osU0FBUyxDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUM7YUFDekIsSUFBSSxDQUNILFNBQVMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQ3hCLFlBQVksQ0FBQyxHQUFHLENBQUMsRUFDakIsTUFBTTs7OztRQUFDLENBQUMsR0FBa0IsRUFBRSxFQUFFLENBQUMsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssUUFBUSxFQUFDLENBQzVEO2FBQ0EsU0FBUzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFO1lBQ2IsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDO1FBQ2YsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7WUF0Q0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztZQUx6QixjQUFjOzs7OztJQU9yQixrQ0FBd0I7O0lBRXhCLHFDQUFjOztJQUVkLHVDQUF5Qjs7Ozs7SUFFYiw2Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFic3RyYWN0VG9hc3RlciB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcclxuaW1wb3J0IHsgQ29uZmlybWF0aW9uIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpcm1hdGlvbic7XHJcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XHJcbmltcG9ydCB7IGZyb21FdmVudCwgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyB0YWtlVW50aWwsIGRlYm91bmNlVGltZSwgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vbW9kZWxzL3RvYXN0ZXInO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvblNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFRvYXN0ZXI8Q29uZmlybWF0aW9uLk9wdGlvbnM+IHtcclxuICBrZXkgPSAnYWJwQ29uZmlybWF0aW9uJztcclxuXHJcbiAgc3RpY2t5ID0gdHJ1ZTtcclxuXHJcbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdCgpO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7XHJcbiAgICBzdXBlcihtZXNzYWdlU2VydmljZSk7XHJcbiAgfVxyXG5cclxuICBzaG93KFxyXG4gICAgbWVzc2FnZTogc3RyaW5nLFxyXG4gICAgdGl0bGU6IHN0cmluZyxcclxuICAgIHNldmVyaXR5OiBUb2FzdGVyLlNldmVyaXR5LFxyXG4gICAgb3B0aW9ucz86IENvbmZpcm1hdGlvbi5PcHRpb25zLFxyXG4gICk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcclxuICAgIHRoaXMubGlzdGVuVG9Fc2NhcGUoKTtcclxuICAgIHJldHVybiBzdXBlci5zaG93KG1lc3NhZ2UsIHRpdGxlLCBzZXZlcml0eSwgb3B0aW9ucyk7XHJcbiAgfVxyXG5cclxuICBjbGVhcihzdGF0dXM/OiBUb2FzdGVyLlN0YXR1cykge1xyXG4gICAgc3VwZXIuY2xlYXIoc3RhdHVzKTtcclxuXHJcbiAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcclxuICB9XHJcblxyXG4gIGxpc3RlblRvRXNjYXBlKCkge1xyXG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAna2V5dXAnKVxyXG4gICAgICAucGlwZShcclxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXHJcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXHJcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkua2V5ID09PSAnRXNjYXBlJyksXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZShfID0+IHtcclxuICAgICAgICB0aGlzLmNsZWFyKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=