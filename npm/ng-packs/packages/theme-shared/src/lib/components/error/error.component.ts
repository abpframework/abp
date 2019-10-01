import { Component, Renderer2, ElementRef } from '@angular/core';

@Component({
  selector: 'abp-error',
  templateUrl: './error.component.html',
  styleUrls: ['error.component.scss']
})
export class ErrorComponent {
  title = 'Oops!';

  details = 'Sorry, an error has occured.';

  renderer: Renderer2;

  elementRef: ElementRef;

  host: any;

  destroy() {
    this.renderer.removeChild(this.host, this.elementRef.nativeElement);
  }
}
