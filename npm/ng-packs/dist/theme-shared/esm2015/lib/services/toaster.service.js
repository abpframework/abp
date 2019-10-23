/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
export class ToasterService extends AbstractToaster {
    /**
     * @param {?} messages
     * @return {?}
     */
    addAll(messages) {
        this.messageService.addAll(messages.map((/**
         * @param {?} message
         * @return {?}
         */
        message => (Object.assign({ key: this.key }, message)))));
    }
}
ToasterService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */ ToasterService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(i0.ɵɵinject(i1.MessageService)); }, token: ToasterService, providedIn: "root" });
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQzs7O0FBSXZELE1BQU0sT0FBTyxjQUFlLFNBQVEsZUFBZTs7Ozs7SUFDakQsTUFBTSxDQUFDLFFBQW1CO1FBQ3hCLElBQUksQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLFFBQVEsQ0FBQyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxpQkFBRyxHQUFHLEVBQUUsSUFBSSxDQUFDLEdBQUcsSUFBSyxPQUFPLEVBQUcsRUFBQyxDQUFDLENBQUM7SUFDdkYsQ0FBQzs7O1lBSkYsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQWJzdHJhY3RUb2FzdGVyIH0gZnJvbSAnLi4vYWJzdHJhY3RzL3RvYXN0ZXInO1xyXG5pbXBvcnQgeyBNZXNzYWdlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlJztcclxuXHJcbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXHJcbmV4cG9ydCBjbGFzcyBUb2FzdGVyU2VydmljZSBleHRlbmRzIEFic3RyYWN0VG9hc3RlciB7XHJcbiAgYWRkQWxsKG1lc3NhZ2VzOiBNZXNzYWdlW10pOiB2b2lkIHtcclxuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuYWRkQWxsKG1lc3NhZ2VzLm1hcChtZXNzYWdlID0+ICh7IGtleTogdGhpcy5rZXksIC4uLm1lc3NhZ2UgfSkpKTtcclxuICB9XHJcbn1cclxuIl19