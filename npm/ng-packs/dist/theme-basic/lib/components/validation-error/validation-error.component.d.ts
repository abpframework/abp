import { Validation, ValidationErrorComponent as ErrorComponent } from '@ngx-validate/core';
export declare class ValidationErrorComponent extends ErrorComponent {
    readonly abpErrors: Validation.Error[] & {
        interpoliteParams?: string[];
    };
}
