import {Component, Inject} from '@angular/core';
import {FORM_PROP_DATA_STREAM, FormProp} from "@abp/ng.theme.shared/extensions";
import {ControlContainer, FormGroup, FormGroupDirective} from "@angular/forms";

@Component({
  selector: 'abp-personal-settings-half-row',
  template: `
    <div class="w-50 d-inline">
      <div class="mb-3 form-group"  >
        <label [attr.for]="name" class="form-label">{{
          displayName | abpLocalization
          }}
        </label>
        <input type="text" [attr.id]="id" class="form-control"  [attr.name]="name" [formControlName]="name"/>
      </div>
    </div>`,
  styles: [],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]

})
export class PersonalSettingsHalfRowComponent {
  public displayName: string;
  public name: string;
  public id: string;
  public formGroup: FormGroup

  constructor(@Inject(FORM_PROP_DATA_STREAM) private propData: FormProp) {
    this.displayName = propData.displayName
    this.name = propData.name
    this.id = propData.id
  }
}
