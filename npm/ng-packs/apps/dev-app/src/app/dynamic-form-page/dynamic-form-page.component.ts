import { ChangeDetectionStrategy, Component, inject, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { DynamicFormComponent, FormFieldConfig } from '@abp/ng.components/dynamic-form';
import { FormConfigService } from './form-config.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-dynamic-form-page',
  templateUrl: './dynamic-form-page.component.html',
  imports: [DynamicFormComponent],
})
export class DynamicFormPageComponent {
  readonly dynamicFormComponent = viewChild(DynamicFormComponent);
  protected readonly formConfigService = inject(FormConfigService);

  readonly formFields = toSignal(this.formConfigService.getFormConfig(), {
    initialValue: [] as FormFieldConfig[],
  });

  submit(formData: any) {
    console.log('✅ Form Submitted Successfully!', formData);
    console.table(formData);

    alert('✅ Form submitted successfully! Check the console for details.');

    this.dynamicFormComponent()?.resetForm();
  }

  cancel() {
    console.log('❌ Form Cancelled');
    alert('Form cancelled');
    this.dynamicFormComponent()?.resetForm();
  }
}
