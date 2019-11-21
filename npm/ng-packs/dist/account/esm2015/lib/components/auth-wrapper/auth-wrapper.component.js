/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input, TemplateRef } from '@angular/core';
export class AuthWrapperComponent {}
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
if (false) {
  /** @type {?} */
  AuthWrapperComponent.prototype.mainContentRef;
  /** @type {?} */
  AuthWrapperComponent.prototype.cancelContentRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC13cmFwcGVyLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2F1dGgtd3JhcHBlci9hdXRoLXdyYXBwZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFNOUQsTUFBTSxPQUFPLG9CQUFvQjs7O1lBSmhDLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsa0JBQWtCO2dCQUM1Qix1Y0FBNEM7YUFDN0M7Ozs2QkFFRSxLQUFLOytCQUdMLEtBQUs7Ozs7SUFITiw4Q0FDaUM7O0lBRWpDLGdEQUNtQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgSW5wdXQsIFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1hdXRoLXdyYXBwZXInLFxuICB0ZW1wbGF0ZVVybDogJy4vYXV0aC13cmFwcGVyLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgQXV0aFdyYXBwZXJDb21wb25lbnQge1xuICBASW5wdXQoKVxuICBtYWluQ29udGVudFJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBASW5wdXQoKVxuICBjYW5jZWxDb250ZW50UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xufVxuIl19
