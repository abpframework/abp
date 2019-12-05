/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/validation-error/validation-error.component.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmFsaWRhdGlvbi1lcnJvci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLmJhc2ljLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdmFsaWRhdGlvbi1lcnJvci92YWxpZGF0aW9uLWVycm9yLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxTQUFTLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEYsT0FBTyxFQUFjLHdCQUF3QixJQUFJLGNBQWMsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBWTVGLE1BQU0sT0FBTyx3QkFBeUIsU0FBUSxjQUFjOzs7O0lBQzFELElBQUksU0FBUztRQUNYLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1lBQUUsT0FBTyxFQUFFLENBQUM7UUFFbkQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRTtZQUM3QixJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU87Z0JBQUUsT0FBTyxLQUFLLENBQUM7O2tCQUUzQixLQUFLLEdBQUcsS0FBSyxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDO1lBRXhDLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO2dCQUNkLHlCQUNLLEtBQUssSUFDUixPQUFPLEVBQUUsS0FBSyxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsQ0FBQyxFQUFFLEtBQUssQ0FBQyxFQUN0QyxpQkFBaUIsRUFBRSxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsQ0FBQyxFQUFFLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxHQUFHLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsSUFDdEY7YUFDSDtZQUVELE9BQU8sS0FBSyxDQUFDO1FBQ2YsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7WUE3QkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxzQkFBc0I7Z0JBQ2hDLFFBQVEsRUFBRTs7OztHQUlUO2dCQUNELGVBQWUsRUFBRSx1QkFBdUIsQ0FBQyxNQUFNO2dCQUMvQyxhQUFhLEVBQUUsaUJBQWlCLENBQUMsSUFBSTthQUN0QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENoYW5nZURldGVjdGlvblN0cmF0ZWd5LCBDb21wb25lbnQsIFZpZXdFbmNhcHN1bGF0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBWYWxpZGF0aW9uLCBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgYXMgRXJyb3JDb21wb25lbnQgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdmFsaWRhdGlvbi1lcnJvcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBjbGFzcz1cImludmFsaWQtZmVlZGJhY2tcIiAqbmdGb3I9XCJsZXQgZXJyb3Igb2YgYWJwRXJyb3JzOyB0cmFja0J5OiB0cmFja0J5Rm5cIj5cbiAgICAgIHt7IGVycm9yLm1lc3NhZ2UgfCBhYnBMb2NhbGl6YXRpb246IGVycm9yLmludGVycG9saXRlUGFyYW1zIH19XG4gICAgPC9kaXY+XG4gIGAsXG4gIGNoYW5nZURldGVjdGlvbjogQ2hhbmdlRGV0ZWN0aW9uU3RyYXRlZ3kuT25QdXNoLFxuICBlbmNhcHN1bGF0aW9uOiBWaWV3RW5jYXBzdWxhdGlvbi5Ob25lLFxufSlcbmV4cG9ydCBjbGFzcyBWYWxpZGF0aW9uRXJyb3JDb21wb25lbnQgZXh0ZW5kcyBFcnJvckNvbXBvbmVudCB7XG4gIGdldCBhYnBFcnJvcnMoKTogVmFsaWRhdGlvbi5FcnJvcltdICYgeyBpbnRlcnBvbGl0ZVBhcmFtcz86IHN0cmluZ1tdIH0ge1xuICAgIGlmICghdGhpcy5lcnJvcnMgfHwgIXRoaXMuZXJyb3JzLmxlbmd0aCkgcmV0dXJuIFtdO1xuXG4gICAgcmV0dXJuIHRoaXMuZXJyb3JzLm1hcChlcnJvciA9PiB7XG4gICAgICBpZiAoIWVycm9yLm1lc3NhZ2UpIHJldHVybiBlcnJvcjtcblxuICAgICAgY29uc3QgaW5kZXggPSBlcnJvci5tZXNzYWdlLmluZGV4T2YoJ1snKTtcblxuICAgICAgaWYgKGluZGV4ID4gLTEpIHtcbiAgICAgICAgcmV0dXJuIHtcbiAgICAgICAgICAuLi5lcnJvcixcbiAgICAgICAgICBtZXNzYWdlOiBlcnJvci5tZXNzYWdlLnNsaWNlKDAsIGluZGV4KSxcbiAgICAgICAgICBpbnRlcnBvbGl0ZVBhcmFtczogZXJyb3IubWVzc2FnZS5zbGljZShpbmRleCArIDEsIGVycm9yLm1lc3NhZ2UubGVuZ3RoIC0gMSkuc3BsaXQoJywnKSxcbiAgICAgICAgfTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIGVycm9yO1xuICAgIH0pO1xuICB9XG59XG4iXX0=