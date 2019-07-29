/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { AbstractToasterClass } from '../abstracts/toaster';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
export class ToasterService extends AbstractToasterClass {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLHNCQUFzQixDQUFDOzs7QUFJNUQsTUFBTSxPQUFPLGNBQWUsU0FBUSxvQkFBb0I7Ozs7O0lBQ3RELE1BQU0sQ0FBQyxRQUFtQjtRQUN4QixJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsaUJBQUcsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQUssT0FBTyxFQUFHLEVBQUMsQ0FBQyxDQUFDO0lBQ3ZGLENBQUM7OztZQUpGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBYnN0cmFjdFRvYXN0ZXJDbGFzcyB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcbmltcG9ydCB7IE1lc3NhZ2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2UnO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIFRvYXN0ZXJTZXJ2aWNlIGV4dGVuZHMgQWJzdHJhY3RUb2FzdGVyQ2xhc3Mge1xuICBhZGRBbGwobWVzc2FnZXM6IE1lc3NhZ2VbXSk6IHZvaWQge1xuICAgIHRoaXMubWVzc2FnZVNlcnZpY2UuYWRkQWxsKG1lc3NhZ2VzLm1hcChtZXNzYWdlID0+ICh7IGtleTogdGhpcy5rZXksIC4uLm1lc3NhZ2UgfSkpKTtcbiAgfVxufVxuIl19