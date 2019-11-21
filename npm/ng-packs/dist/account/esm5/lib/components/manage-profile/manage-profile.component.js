/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
var ManageProfileComponent = /** @class */ (function() {
  function ManageProfileComponent() {
    this.selectedTab = 0;
  }
  ManageProfileComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-manage-profile',
          template:
            '<div id="AbpContentToolbar"></div>\n\n<div class="card border-0 shadow-sm">\n  <div class="card-body">\n    <div class="row">\n      <div class="col-3">\n        <ul class="nav flex-column nav-pills" id="nav-tab" role="tablist">\n          <li class="nav-item" (click)="selectedTab = 0">\n            <a class="nav-link" [ngClass]="{ active: selectedTab === 0 }" role="tab" href="javascript:void(0)">{{\n              \'AbpUi::ChangePassword\' | abpLocalization\n            }}</a>\n          </li>\n          <li class="nav-item" (click)="selectedTab = 1">\n            <a class="nav-link" [ngClass]="{ active: selectedTab === 1 }" role="tab" href="javascript:void(0)">{{\n              \'AbpAccount::PersonalSettings\' | abpLocalization\n            }}</a>\n          </li>\n        </ul>\n      </div>\n      <div class="col-9">\n        <div class="tab-content" *ngIf="selectedTab === 0" [@fadeIn]>\n          <div class="tab-pane active" role="tabpanel">\n            <h4>\n              {{ \'AbpIdentity::ChangePassword\' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-change-password-form></abp-change-password-form>\n          </div>\n        </div>\n        <div class="tab-content" *ngIf="selectedTab === 1" [@fadeIn]>\n          <div class="tab-pane active" role="tabpanel">\n            <h4>\n              {{ \'AbpIdentity::PersonalSettings\' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-personal-settings-form></abp-personal-settings-form>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n',
          animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])],
        },
      ],
    },
  ];
  return ManageProfileComponent;
})();
export { ManageProfileComponent };
if (false) {
  /** @type {?} */
  ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDeEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUxQztJQUFBO1FBTUUsZ0JBQVcsR0FBRyxDQUFDLENBQUM7SUFDbEIsQ0FBQzs7Z0JBUEEsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxvQkFBb0I7b0JBQzlCLCtuREFBOEM7b0JBQzlDLFVBQVUsRUFBRSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztpQkFDOUU7O0lBR0QsNkJBQUM7Q0FBQSxBQVBELElBT0M7U0FGWSxzQkFBc0I7OztJQUNqQyw2Q0FBZ0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBmYWRlSW4gfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyB0cmFuc2l0aW9uLCB0cmlnZ2VyLCB1c2VBbmltYXRpb24gfSBmcm9tICdAYW5ndWxhci9hbmltYXRpb25zJztcbmltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbWFuYWdlLXByb2ZpbGUnLFxuICB0ZW1wbGF0ZVVybDogJy4vbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50Lmh0bWwnLFxuICBhbmltYXRpb25zOiBbdHJpZ2dlcignZmFkZUluJywgW3RyYW5zaXRpb24oJzplbnRlcicsIHVzZUFuaW1hdGlvbihmYWRlSW4pKV0pXSxcbn0pXG5leHBvcnQgY2xhc3MgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCB7XG4gIHNlbGVjdGVkVGFiID0gMDtcbn1cbiJdfQ==
