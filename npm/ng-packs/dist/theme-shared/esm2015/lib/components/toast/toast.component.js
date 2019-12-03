/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ToastComponent {
}
ToastComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-toast',
                // tslint:disable-next-line: component-max-inline-declarations
                template: `
    <p-toast position="bottom-right" key="abpToast" styleClass="abp-toast" [baseZIndex]="1000">
      <ng-template let-message pTemplate="message">
        <span
          class="ui-toast-icon pi"
          [ngClass]="{
            'pi-info-circle': message.severity === 'info',
            'pi-exclamation-triangle': message.severity === 'warn',
            'pi-times': message.severity === 'error',
            'pi-check': message.severity === 'success'
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidG9hc3QuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90b2FzdC90b2FzdC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUF5QjFDLE1BQU0sT0FBTyxjQUFjOzs7WUF2QjFCLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVzs7Z0JBRXJCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7O0dBa0JUO2FBQ0YiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRvYXN0JyxcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcbiAgdGVtcGxhdGU6IGBcbiAgICA8cC10b2FzdCBwb3NpdGlvbj1cImJvdHRvbS1yaWdodFwiIGtleT1cImFicFRvYXN0XCIgc3R5bGVDbGFzcz1cImFicC10b2FzdFwiIFtiYXNlWkluZGV4XT1cIjEwMDBcIj5cbiAgICAgIDxuZy10ZW1wbGF0ZSBsZXQtbWVzc2FnZSBwVGVtcGxhdGU9XCJtZXNzYWdlXCI+XG4gICAgICAgIDxzcGFuXG4gICAgICAgICAgY2xhc3M9XCJ1aS10b2FzdC1pY29uIHBpXCJcbiAgICAgICAgICBbbmdDbGFzc109XCJ7XG4gICAgICAgICAgICAncGktaW5mby1jaXJjbGUnOiBtZXNzYWdlLnNldmVyaXR5ID09PSAnaW5mbycsXG4gICAgICAgICAgICAncGktZXhjbGFtYXRpb24tdHJpYW5nbGUnOiBtZXNzYWdlLnNldmVyaXR5ID09PSAnd2FybicsXG4gICAgICAgICAgICAncGktdGltZXMnOiBtZXNzYWdlLnNldmVyaXR5ID09PSAnZXJyb3InLFxuICAgICAgICAgICAgJ3BpLWNoZWNrJzogbWVzc2FnZS5zZXZlcml0eSA9PT0gJ3N1Y2Nlc3MnXG4gICAgICAgICAgfVwiXG4gICAgICAgID48L3NwYW4+XG4gICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1tZXNzYWdlLXRleHQtY29udGVudFwiPlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJ1aS10b2FzdC1zdW1tYXJ5XCI+e3sgbWVzc2FnZS5zdW1tYXJ5IHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLnRpdGxlTG9jYWxpemF0aW9uUGFyYW1zIH19PC9kaXY+XG4gICAgICAgICAgPGRpdiBjbGFzcz1cInVpLXRvYXN0LWRldGFpbFwiPnt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX08L2Rpdj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L25nLXRlbXBsYXRlPlxuICAgIDwvcC10b2FzdD5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgVG9hc3RDb21wb25lbnQge31cbiJdfQ==