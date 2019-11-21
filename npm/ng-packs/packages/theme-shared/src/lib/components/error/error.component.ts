import { Component, Renderer2, ElementRef } from '@angular/core';
import { Config } from '@abp/ng.core';

@Component({
  selector: 'abp-error',
  templateUrl: './error.component.html',
  styleUrls: ['error.component.scss'],
})
export class ErrorComponent {
  title: string | Config.LocalizationWithDefault = 'Oops!';

  details: string | Config.LocalizationWithDefault = 'Sorry, an error has occured.';

  renderer: Renderer2;

  elementRef: ElementRef;

  host: any;

  destroy() {
    this.renderer.removeChild(this.host, this.elementRef.nativeElement);
  }
}
