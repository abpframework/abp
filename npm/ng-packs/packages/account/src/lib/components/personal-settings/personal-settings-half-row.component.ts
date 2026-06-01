import { Component, inject } from '@angular/core';
import {
  EXTENSIONS_FORM_PROP,
  FormProp,
  EXTENSIBLE_FORM_VIEW_PROVIDER,
} from '@abp/ng.components/extensible';
import { ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-personal-settings-half-row',
  template: ` <div class="w-50 d-inline">
    <label [attr.for]="name" class="form-label">{{ displayName | abpLocalization }} </label>
    <input
      type="text"
      [attr.id]="id"
      class="form-control"
      [attr.name]="name"
      [formControlName]="name"
    />
  </div>`,
  styles: [],
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
  imports: [ReactiveFormsModule, LocalizationPipe],
})
export class PersonalSettingsHalfRowComponent {
  private propData = inject<FormProp>(EXTENSIONS_FORM_PROP);

  public displayName: string;
  public name: string;
  public id: string;
  public formGroup!: UntypedFormGroup;

  constructor() {
    const propData = this.propData;

    this.displayName = propData.displayName;
    this.name = propData.name;
    this.id = propData.id || '';
  }
}
