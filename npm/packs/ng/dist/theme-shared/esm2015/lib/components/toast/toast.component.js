/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ToastComponent {
}
ToastComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-toast',
                template: `
    <p-toast position="bottom-right" key="abpToast" [baseZIndex]="1000">
      <ng-template let-message pTemplate="message">
        <span
          class="ui-toast-icon pi"
          [ngClass]="{
            'pi-info-circle': message.severity == 'info',
            'pi-exclamation-triangle': message.severity == 'warn',
            'pi-times': message.severity == 'error',
            'pi-check': message.severity == 'success'
          }"
        ></span>
        <div class="ui-toast-message-text-content">
          <div class="ui-toast-summary">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>
          <div class="ui-toast-detail">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>
        </div>
      </ng-template>
    </p-toast>
  `
            }] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUF3QjFDLE1BQU0sT0FBTyxjQUFjOzs7WUF0QjFCLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVztnQkFDckIsUUFBUSxFQUFFOzs7Ozs7Ozs7Ozs7Ozs7Ozs7R0FrQlQ7YUFDRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdG9hc3QnLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxwLXRvYXN0IHBvc2l0aW9uPVwiYm90dG9tLXJpZ2h0XCIga2V5PVwiYWJwVG9hc3RcIiBbYmFzZVpJbmRleF09XCIxMDAwXCI+XG4gICAgICA8bmctdGVtcGxhdGUgbGV0LW1lc3NhZ2UgcFRlbXBsYXRlPVwibWVzc2FnZVwiPlxuICAgICAgICA8c3BhblxuICAgICAgICAgIGNsYXNzPVwidWktdG9hc3QtaWNvbiBwaVwiXG4gICAgICAgICAgW25nQ2xhc3NdPVwie1xuICAgICAgICAgICAgJ3BpLWluZm8tY2lyY2xlJzogbWVzc2FnZS5zZXZlcml0eSA9PSAnaW5mbycsXG4gICAgICAgICAgICAncGktZXhjbGFtYXRpb24tdHJpYW5nbGUnOiBtZXNzYWdlLnNldmVyaXR5ID09ICd3YXJuJyxcbiAgICAgICAgICAgICdwaS10aW1lcyc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ2Vycm9yJyxcbiAgICAgICAgICAgICdwaS1jaGVjayc6IG1lc3NhZ2Uuc2V2ZXJpdHkgPT0gJ3N1Y2Nlc3MnXG4gICAgICAgICAgfVwiXG4gICAgICAgID48L3NwYW4+XG4gICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1tZXNzYWdlLXRleHQtY29udGVudFwiPlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1zdW1tYXJ5XCI+e3sgbWVzc2FnZS5zdW1tYXJ5IHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLnRpdGxlTG9jYWxpemF0aW9uUGFyYW1zIH19PC9kaXY+XG4gICAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LWRldGFpbFwiPnt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX08L2Rpdj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L25nLXRlbXBsYXRlPlxuICAgIDwvcC10b2FzdD5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgVG9hc3RDb21wb25lbnQge31cbiJdfQ==