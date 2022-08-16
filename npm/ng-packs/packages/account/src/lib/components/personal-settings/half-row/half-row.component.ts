import {Component, Inject} from '@angular/core';
import {FORM_PROP_DATA_STREAM, FormProp} from "@abp/ng.theme.shared/extensions";
import {ControlContainer, FormGroup, FormGroupDirective} from "@angular/forms";

@Component({
  selector: 'abp-half-row',
  template: `
    <div class="col col-md-6">
      <div class="mb-3 form-group"  >
        <label for="surname" class="form-label">{{
          displayName | abpLocalization
          }}
          {{name}}
        </label>
        <input type="text" [attr.id]="id" class="form-control"  [attr.name]="name" [formControlName]="name"/>
      </div>
    </div>`,
  styles: [],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]

})
export class HalfRowComponent {
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
