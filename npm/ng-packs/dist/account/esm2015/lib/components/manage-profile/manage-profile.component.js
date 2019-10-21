/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ManageProfileComponent {
    constructor() {
        this.selectedTab = 0;
    }
    /**
     * @return {?}
     */
    ngOnInit() { }
}
ManageProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-manage-profile',
                template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\"></div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\"></div>\n  <div class=\"col\"></div>\n</div>\n\n<div id=\"ManageProfileWrapper\">\n  <div class=\"row\">\n    <div class=\"col-3\">\n      <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 0\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\">{{\n            'AbpUi::ChangePassword' | abpLocalization\n          }}</a>\n        </li>\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 1\">\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\">{{\n            'AbpAccount::PersonalSettings' | abpLocalization\n          }}</a>\n        </li>\n      </ul>\n    </div>\n    <div class=\"col-9\">\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 0\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-change-password-form></abp-change-password-form>\n        </div>\n      </div>\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 1\">\n        <div class=\"tab-pane fade show active\" role=\"tabpanel\">\n          <abp-personal-settings-form></abp-personal-settings-form>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
ManageProfileComponent.ctorParameters = () => [];
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBTWxELE1BQU0sT0FBTyxzQkFBc0I7SUFHakM7UUFGQSxnQkFBVyxHQUFHLENBQUMsQ0FBQztJQUVELENBQUM7Ozs7SUFFaEIsUUFBUSxLQUFVLENBQUM7OztZQVRwQixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLG9CQUFvQjtnQkFDOUIsKzJDQUE4QzthQUMvQzs7Ozs7O0lBRUMsNkNBQWdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLW1hbmFnZS1wcm9maWxlJyxcbiAgdGVtcGxhdGVVcmw6ICcuL21hbmFnZS1wcm9maWxlLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIHNlbGVjdGVkVGFiID0gMDtcblxuICBjb25zdHJ1Y3RvcigpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7fVxufVxuIl19