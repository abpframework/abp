/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3Rlci5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdG9hc3Rlci5zZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQzs7O0FBSXZELE1BQU0sT0FBTyxjQUFlLFNBQVEsZUFBZTs7Ozs7SUFDakQsTUFBTSxDQUFDLFFBQW1CO1FBQ3hCLElBQUksQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLFFBQVEsQ0FBQyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxpQkFBRyxHQUFHLEVBQUUsSUFBSSxDQUFDLEdBQUcsSUFBSyxPQUFPLEVBQUcsRUFBQyxDQUFDLENBQUM7SUFDdkYsQ0FBQzs7O1lBSkYsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFic3RyYWN0VG9hc3RlciB9IGZyb20gJy4uL2Fic3RyYWN0cy90b2FzdGVyJztcbmltcG9ydCB7IE1lc3NhZ2UgfSBmcm9tICdwcmltZW5nL2NvbXBvbmVudHMvY29tbW9uL21lc3NhZ2UnO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIFRvYXN0ZXJTZXJ2aWNlIGV4dGVuZHMgQWJzdHJhY3RUb2FzdGVyIHtcbiAgYWRkQWxsKG1lc3NhZ2VzOiBNZXNzYWdlW10pOiB2b2lkIHtcbiAgICB0aGlzLm1lc3NhZ2VTZXJ2aWNlLmFkZEFsbChtZXNzYWdlcy5tYXAobWVzc2FnZSA9PiAoeyBrZXk6IHRoaXMua2V5LCAuLi5tZXNzYWdlIH0pKSk7XG4gIH1cbn1cbiJdfQ==