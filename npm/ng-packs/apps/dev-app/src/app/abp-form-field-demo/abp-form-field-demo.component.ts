import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AbpFormFieldComponent, AbpFormFieldLabelComponent } from '@abp/ng.components/abp-form-field';
import { CardComponent, CardBodyComponent, CardHeaderComponent } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { AbpInputComponent } from '@abp/ng.components/abp-input';

@Component({
  selector: 'app-abp-form-field-demo',
  templateUrl: './abp-form-field-demo.component.html',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AbpFormFieldComponent,
    CardComponent,
    CardBodyComponent,
    CardHeaderComponent,
    AbpInputComponent,
    AbpFormFieldLabelComponent,
  ],
})
export class AbpFormFieldDemoComponent {
  private fb = inject(FormBuilder);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    age: [null, [Validators.required, Validators.min(18)]],
    description: ['', [Validators.maxLength(200)]],
    agree: [false, [Validators.requiredTrue]],
  });

  submit() {
    if (this.form.valid) {
      console.log(this.form.value);
    } else {
      console.log('Form is invalid');
    }
  }
}
