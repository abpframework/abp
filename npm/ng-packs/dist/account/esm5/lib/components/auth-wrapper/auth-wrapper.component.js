/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input, TemplateRef } from '@angular/core';
var AuthWrapperComponent = /** @class */ (function() {
  function AuthWrapperComponent() {}
  AuthWrapperComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-auth-wrapper',
          template:
            '<div class="row">\n  <div class="mx-auto col col-md-5">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class="abp-account-container">\n      <div class="card mt-3 shadow-sm rounded">\n        <div class="card-body p-5">\n          <ng-content *ngTemplateOutlet="mainContentRef"></ng-content>\n        </div>\n        <ng-content *ngTemplateOutlet="cancelContentRef"></ng-content>\n      </div>\n    </div>\n  </div>\n</div>\n',
        },
      ],
    },
  ];
  AuthWrapperComponent.propDecorators = {
    mainContentRef: [{ type: Input }],
    cancelContentRef: [{ type: Input }],
  };
  return AuthWrapperComponent;
})();
export { AuthWrapperComponent };
if (false) {
  /** @type {?} */
  AuthWrapperComponent.prototype.mainContentRef;
  /** @type {?} */
  AuthWrapperComponent.prototype.cancelContentRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC13cmFwcGVyLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2F1dGgtd3JhcHBlci9hdXRoLXdyYXBwZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFOUQ7SUFBQTtJQVVBLENBQUM7O2dCQVZBLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsa0JBQWtCO29CQUM1Qix1Y0FBNEM7aUJBQzdDOzs7aUNBRUUsS0FBSzttQ0FHTCxLQUFLOztJQUVSLDJCQUFDO0NBQUEsQUFWRCxJQVVDO1NBTlksb0JBQW9COzs7SUFDL0IsOENBQ2lDOztJQUVqQyxnREFDbUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBUZW1wbGF0ZVJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtYXV0aC13cmFwcGVyJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2F1dGgtd3JhcHBlci5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIEF1dGhXcmFwcGVyQ29tcG9uZW50IHtcbiAgQElucHV0KClcbiAgbWFpbkNvbnRlbnRSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQElucHV0KClcbiAgY2FuY2VsQ29udGVudFJlZjogVGVtcGxhdGVSZWY8YW55Pjtcbn1cbiJdfQ==
