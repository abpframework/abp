/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/toaster.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService } from 'primeng/components/common/messageservice';
import * as i0 from "@angular/core";
import * as i1 from "primeng/components/common/messageservice";
export class ToasterService extends AbstractToaster {
    /**
     * @param {?} messageService
     */
    constructor(messageService) {
        super(messageService);
        this.messageService = messageService;
    }
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
/** @nocollapse */
ToasterService.ctorParameters = () => [
    { type: MessageService }
];
/** @nocollapse */ ToasterService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(i0.ɵɵinject(i1.MessageService)); }, token: ToasterService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @protected
     */
    ToasterService.prototype.messageService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFFdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDBDQUEwQyxDQUFDOzs7QUFHMUUsTUFBTSxPQUFPLGNBQWUsU0FBUSxlQUFlOzs7O0lBQ2pELFlBQXNCLGNBQThCO1FBQ2xELEtBQUssQ0FBQyxjQUFjLENBQUMsQ0FBQztRQURGLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUVwRCxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxRQUFtQjtRQUN4QixJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsaUJBQUcsR0FBRyxFQUFFLElBQUksQ0FBQyxHQUFHLElBQUssT0FBTyxFQUFHLEVBQUMsQ0FBQyxDQUFDO0lBQ3ZGLENBQUM7OztZQVJGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7WUFGekIsY0FBYzs7Ozs7Ozs7SUFJVCx3Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBYnN0cmFjdFRvYXN0ZXIgfSBmcm9tICcuLi9hYnN0cmFjdHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBNZXNzYWdlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlJztcbmltcG9ydCB7IE1lc3NhZ2VTZXJ2aWNlIH0gZnJvbSAncHJpbWVuZy9jb21wb25lbnRzL2NvbW1vbi9tZXNzYWdlc2VydmljZSc7XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgVG9hc3RlclNlcnZpY2UgZXh0ZW5kcyBBYnN0cmFjdFRvYXN0ZXIge1xuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgbWVzc2FnZVNlcnZpY2U6IE1lc3NhZ2VTZXJ2aWNlKSB7XG4gICAgc3VwZXIobWVzc2FnZVNlcnZpY2UpO1xuICB9XG5cbiAgYWRkQWxsKG1lc3NhZ2VzOiBNZXNzYWdlW10pOiB2b2lkIHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZEFsbChtZXNzYWdlcy5tYXAobWVzc2FnZSA9PiAoeyBrZXk6IHRoaXMua2V5LCAuLi5tZXNzYWdlIH0pKSk7XG4gIH1cbn1cbiJdfQ==