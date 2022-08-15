import {Component, Inject } from '@angular/core';
import {FORM_PROP_DATA_STREAM, FormProp} from "@abp/ng.theme.shared/extensions";

@Component({
  selector: 'abp-hello',
  template: `<p>hello works! {{name | abpLocalization}}</p>`,
  styles: [],
})
export class HelloComponent  {
  name:string;
  constructor(@Inject(FORM_PROP_DATA_STREAM) private propData:FormProp) {
    this.name = propData.displayName
  }

}
