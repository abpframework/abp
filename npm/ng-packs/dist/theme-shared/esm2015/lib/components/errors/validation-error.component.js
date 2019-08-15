/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { ValidationErrorComponent as ErrorComponent } from '@ngx-validate/core';
export class ValidationErrorComponent extends ErrorComponent {
    /**
     * @return {?}
     */
    get abpErrors() {
        return this.validationErrors.map((/**
         * @param {?} error
         * @return {?}
         */
        error => {
            if (!error.message)
                return error;
            /** @type {?} */
            const index = error.message.indexOf('[');
            if (index > -1) {
                return Object.assign({}, error, { message: error.message.slice(0, index), interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(',') });
            }
            return error;
        }));
    }
}
ValidationErrorComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-validation-error',
                template: `
    <div class="invalid-feedback" *ngFor="let error of abpErrors; trackBy: trackByFn">
      {{ error.message | abpLocalization: error.interpoliteParams }}
    </div>
  `,
                changeDetection: ChangeDetectionStrategy.OnPush,
                encapsulation: ViewEncapsulation.None
            }] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2Vycm9ycy92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLHVCQUF1QixFQUFFLFNBQVMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0RixPQUFPLEVBQWMsd0JBQXdCLElBQUksY0FBYyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFZNUYsTUFBTSxPQUFPLHdCQUF5QixTQUFRLGNBQWM7Ozs7SUFDMUQsSUFBSSxTQUFTO1FBQ1gsT0FBTyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsR0FBRzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFO1lBQ3ZDLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTztnQkFBRSxPQUFPLEtBQUssQ0FBQzs7a0JBRTNCLEtBQUssR0FBRyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUM7WUFFeEMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7Z0JBQ2QseUJBQ0ssS0FBSyxJQUNSLE9BQU8sRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDLEVBQ3RDLGlCQUFpQixFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxDQUFDLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLEdBQUcsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxJQUN0RjthQUNIO1lBRUQsT0FBTyxLQUFLLENBQUM7UUFDZixDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7OztZQTNCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLHNCQUFzQjtnQkFDaEMsUUFBUSxFQUFFOzs7O0dBSVQ7Z0JBQ0QsZUFBZSxFQUFFLHVCQUF1QixDQUFDLE1BQU07Z0JBQy9DLGFBQWEsRUFBRSxpQkFBaUIsQ0FBQyxJQUFJO2FBQ3RDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCwgVmlld0VuY2Fwc3VsYXRpb24gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFZhbGlkYXRpb24sIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBhcyBFcnJvckNvbXBvbmVudCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC12YWxpZGF0aW9uLWVycm9yJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGNsYXNzPVwiaW52YWxpZC1mZWVkYmFja1wiICpuZ0Zvcj1cImxldCBlcnJvciBvZiBhYnBFcnJvcnM7IHRyYWNrQnk6IHRyYWNrQnlGblwiPlxuICAgICAge3sgZXJyb3IubWVzc2FnZSB8IGFicExvY2FsaXphdGlvbjogZXJyb3IuaW50ZXJwb2xpdGVQYXJhbXMgfX1cbiAgICA8L2Rpdj5cbiAgYCxcbiAgY2hhbmdlRGV0ZWN0aW9uOiBDaGFuZ2VEZXRlY3Rpb25TdHJhdGVneS5PblB1c2gsXG4gIGVuY2Fwc3VsYXRpb246IFZpZXdFbmNhcHN1bGF0aW9uLk5vbmUsXG59KVxuZXhwb3J0IGNsYXNzIFZhbGlkYXRpb25FcnJvckNvbXBvbmVudCBleHRlbmRzIEVycm9yQ29tcG9uZW50IHtcbiAgZ2V0IGFicEVycm9ycygpOiBWYWxpZGF0aW9uLkVycm9yW10gJiB7IGludGVycG9saXRlUGFyYW1zPzogc3RyaW5nW10gfSB7XG4gICAgcmV0dXJuIHRoaXMudmFsaWRhdGlvbkVycm9ycy5tYXAoZXJyb3IgPT4ge1xuICAgICAgaWYgKCFlcnJvci5tZXNzYWdlKSByZXR1cm4gZXJyb3I7XG5cbiAgICAgIGNvbnN0IGluZGV4ID0gZXJyb3IubWVzc2FnZS5pbmRleE9mKCdbJyk7XG5cbiAgICAgIGlmIChpbmRleCA+IC0xKSB7XG4gICAgICAgIHJldHVybiB7XG4gICAgICAgICAgLi4uZXJyb3IsXG4gICAgICAgICAgbWVzc2FnZTogZXJyb3IubWVzc2FnZS5zbGljZSgwLCBpbmRleCksXG4gICAgICAgICAgaW50ZXJwb2xpdGVQYXJhbXM6IGVycm9yLm1lc3NhZ2Uuc2xpY2UoaW5kZXggKyAxLCBlcnJvci5tZXNzYWdlLmxlbmd0aCAtIDEpLnNwbGl0KCcsJyksXG4gICAgICAgIH07XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBlcnJvcjtcbiAgICB9KTtcbiAgfVxufVxuIl19