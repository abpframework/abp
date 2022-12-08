import { Component, Inject } from '@angular/core';
import {
  EXTENSIONS_FORM_PROP,
  FormProp,
  EXTENSIBLE_FORM_VIEW_PROVIDER,
} from '@abp/ng.theme.shared/extensions';
import { UntypedFormGroup } from '@angular/forms';

@Component({
  selector: 'abp-personal-settings-half-row',
  template: ` <div class="w-50 d-inline">
    <div class="mb-3">
      <label [attr.for]="name" class="form-label">{{ displayName | abpLocalization }} </label>
      <input
        type="text"
        [attr.id]="id"
        class="form-control"
        [attr.name]="name"
        [formControlName]="name"
      />
    </div>
  </div>`,
  styles: [],
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
})
export class PersonalSettingsHalfRowComponent {
  public displayName: string;
  public name: string;
  public id: string;
  public formGroup: UntypedFormGroup;

  constructor(@Inject(EXTENSIONS_FORM_PROP) private propData: FormProp) {
    this.displayName = propData.displayName;
    this.name = propData.name;
    this.id = propData.id;
  }
}
