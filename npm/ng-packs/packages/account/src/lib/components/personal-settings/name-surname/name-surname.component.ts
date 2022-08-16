import { Component } from '@angular/core';
import {ControlContainer, FormGroupDirective} from "@angular/forms";

@Component({
  selector: 'abp-name-surname',
  template: `<div class="row">
    <div class="col col-md-6">
      <div class="mb-3 form-group">
        <label for="name" class="form-label">{{
          'AbpIdentity::DisplayName:Name' | abpLocalization
          }}</label
        ><input type="text" id="name" class="form-control" formControlName="name" />
      </div>
    </div>
    <div class="col col-md-6">
      <div class="mb-3 form-group">
        <label for="surname" class="form-label">{{
          'AbpIdentity::DisplayName:Surname' | abpLocalization
          }}</label
        ><input type="text" id="surname" class="form-control" formControlName="surname" />
      </div>
    </div>
  </div>`,
  styles: [],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]

})
export class NameSurnameComponent  {
  constructor() {}
}
