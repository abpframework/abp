/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
export class ManageProfileComponent {
  constructor() {
    this.selectedTab = 0;
  }
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
if (false) {
  /** @type {?} */
  ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDeEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQU8xQyxNQUFNLE9BQU8sc0JBQXNCO0lBTG5DO1FBTUUsZ0JBQVcsR0FBRyxDQUFDLENBQUM7SUFDbEIsQ0FBQzs7O1lBUEEsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxvQkFBb0I7Z0JBQzlCLCtuREFBOEM7Z0JBQzlDLFVBQVUsRUFBRSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQzthQUM5RTs7OztJQUVDLDZDQUFnQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGZhZGVJbiB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IHRyYW5zaXRpb24sIHRyaWdnZXIsIHVzZUFuaW1hdGlvbiB9IGZyb20gJ0Bhbmd1bGFyL2FuaW1hdGlvbnMnO1xuaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1tYW5hZ2UtcHJvZmlsZScsXG4gIHRlbXBsYXRlVXJsOiAnLi9tYW5hZ2UtcHJvZmlsZS5jb21wb25lbnQuaHRtbCcsXG4gIGFuaW1hdGlvbnM6IFt0cmlnZ2VyKCdmYWRlSW4nLCBbdHJhbnNpdGlvbignOmVudGVyJywgdXNlQW5pbWF0aW9uKGZhZGVJbikpXSldLFxufSlcbmV4cG9ydCBjbGFzcyBNYW5hZ2VQcm9maWxlQ29tcG9uZW50IHtcbiAgc2VsZWN0ZWRUYWIgPSAwO1xufVxuIl19
