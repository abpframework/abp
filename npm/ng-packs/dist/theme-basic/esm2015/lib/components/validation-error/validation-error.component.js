/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { ValidationErrorComponent as ErrorComponent } from '@ngx-validate/core';
export class ValidationErrorComponent extends ErrorComponent {
    /**
     * @return {?}
     */
    get abpErrors() {
        if (!this.errors || !this.errors.length)
            return [];
        return this.errors.map((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLHVCQUF1QixFQUFFLFNBQVMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN0RixPQUFPLEVBQWMsd0JBQXdCLElBQUksY0FBYyxFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFZNUYsTUFBTSxPQUFPLHdCQUF5QixTQUFRLGNBQWM7Ozs7SUFDMUQsSUFBSSxTQUFTO1FBQ1gsSUFBSSxDQUFDLElBQUksQ0FBQyxNQUFNLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07WUFBRSxPQUFPLEVBQUUsQ0FBQztRQUVuRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFO1lBQzdCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTztnQkFBRSxPQUFPLEtBQUssQ0FBQzs7a0JBRTNCLEtBQUssR0FBRyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUM7WUFFeEMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7Z0JBQ2QseUJBQ0ssS0FBSyxJQUNSLE9BQU8sRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDLEVBQ3RDLGlCQUFpQixFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLEtBQUssR0FBRyxDQUFDLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLEdBQUcsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxJQUN0RjthQUNIO1lBRUQsT0FBTyxLQUFLLENBQUM7UUFDZixDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7OztZQTdCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLHNCQUFzQjtnQkFDaEMsUUFBUSxFQUFFOzs7O0dBSVQ7Z0JBQ0QsZUFBZSxFQUFFLHVCQUF1QixDQUFDLE1BQU07Z0JBQy9DLGFBQWEsRUFBRSxpQkFBaUIsQ0FBQyxJQUFJO2FBQ3RDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3ksIENvbXBvbmVudCwgVmlld0VuY2Fwc3VsYXRpb24gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgVmFsaWRhdGlvbiwgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGFzIEVycm9yQ29tcG9uZW50IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXZhbGlkYXRpb24tZXJyb3InLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8ZGl2IGNsYXNzPVwiaW52YWxpZC1mZWVkYmFja1wiICpuZ0Zvcj1cImxldCBlcnJvciBvZiBhYnBFcnJvcnM7IHRyYWNrQnk6IHRyYWNrQnlGblwiPlxyXG4gICAgICB7eyBlcnJvci5tZXNzYWdlIHwgYWJwTG9jYWxpemF0aW9uOiBlcnJvci5pbnRlcnBvbGl0ZVBhcmFtcyB9fVxyXG4gICAgPC9kaXY+XHJcbiAgYCxcclxuICBjaGFuZ2VEZXRlY3Rpb246IENoYW5nZURldGVjdGlvblN0cmF0ZWd5Lk9uUHVzaCxcclxuICBlbmNhcHN1bGF0aW9uOiBWaWV3RW5jYXBzdWxhdGlvbi5Ob25lLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVmFsaWRhdGlvbkVycm9yQ29tcG9uZW50IGV4dGVuZHMgRXJyb3JDb21wb25lbnQge1xyXG4gIGdldCBhYnBFcnJvcnMoKTogVmFsaWRhdGlvbi5FcnJvcltdICYgeyBpbnRlcnBvbGl0ZVBhcmFtcz86IHN0cmluZ1tdIH0ge1xyXG4gICAgaWYgKCF0aGlzLmVycm9ycyB8fCAhdGhpcy5lcnJvcnMubGVuZ3RoKSByZXR1cm4gW107XHJcblxyXG4gICAgcmV0dXJuIHRoaXMuZXJyb3JzLm1hcChlcnJvciA9PiB7XHJcbiAgICAgIGlmICghZXJyb3IubWVzc2FnZSkgcmV0dXJuIGVycm9yO1xyXG5cclxuICAgICAgY29uc3QgaW5kZXggPSBlcnJvci5tZXNzYWdlLmluZGV4T2YoJ1snKTtcclxuXHJcbiAgICAgIGlmIChpbmRleCA+IC0xKSB7XHJcbiAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgIC4uLmVycm9yLFxyXG4gICAgICAgICAgbWVzc2FnZTogZXJyb3IubWVzc2FnZS5zbGljZSgwLCBpbmRleCksXHJcbiAgICAgICAgICBpbnRlcnBvbGl0ZVBhcmFtczogZXJyb3IubWVzc2FnZS5zbGljZShpbmRleCArIDEsIGVycm9yLm1lc3NhZ2UubGVuZ3RoIC0gMSkuc3BsaXQoJywnKSxcclxuICAgICAgICB9O1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gZXJyb3I7XHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19