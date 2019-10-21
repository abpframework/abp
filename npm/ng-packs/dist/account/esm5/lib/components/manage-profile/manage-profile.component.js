/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
var ManageProfileComponent = /** @class */ (function () {
    function ManageProfileComponent() {
        this.selectedTab = 0;
    }
    /**
     * @return {?}
     */
    ManageProfileComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () { };
    ManageProfileComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-manage-profile',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\"></div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\"></div>\n  <div class=\"col\"></div>\n</div>\n\n<div id=\"ManageProfileWrapper\">\n  <div class=\"row\">\n    <div class=\"col-3\">\n      <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 0\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\">{{\n            'AbpUi::ChangePassword' | abpLocalization\n          }}</a>\n        </li>\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 1\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\">{{\n            'AbpAccount::PersonalSettings' | abpLocalization\n          }}</a>\n        </li>\n      </ul>\n    </div>\n    <div class=\"col-9\">\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 0\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-change-password-form></abp-change-password-form>\n        </div>\n      </div>\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 1\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-personal-settings-form></abp-personal-settings-form>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    ManageProfileComponent.ctorParameters = function () { return []; };
    return ManageProfileComponent;
}());
export { ManageProfileComponent };
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBRWxEO0lBT0U7UUFGQSxnQkFBVyxHQUFHLENBQUMsQ0FBQztJQUVELENBQUM7Ozs7SUFFaEIseUNBQVE7OztJQUFSLGNBQWtCLENBQUM7O2dCQVRwQixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLG9CQUFvQjtvQkFDOUIsKzJDQUE4QztpQkFDL0M7Ozs7SUFPRCw2QkFBQztDQUFBLEFBVkQsSUFVQztTQU5ZLHNCQUFzQjs7O0lBQ2pDLDZDQUFnQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1tYW5hZ2UtcHJvZmlsZScsXG4gIHRlbXBsYXRlVXJsOiAnLi9tYW5hZ2UtcHJvZmlsZS5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIE1hbmFnZVByb2ZpbGVDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzZWxlY3RlZFRhYiA9IDA7XG5cbiAgY29uc3RydWN0b3IoKSB7fVxuXG4gIG5nT25Jbml0KCk6IHZvaWQge31cbn1cbiJdfQ==