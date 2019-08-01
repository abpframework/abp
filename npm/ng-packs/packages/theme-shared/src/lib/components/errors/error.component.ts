import { Component, Renderer2, ElementRef } from '@angular/core';

@Component({
  selector: 'abp-error',
  template: `
    <div class="error">
      <button id="abp-close-button mr-2" type="button" class="close" (click)="destroy()">
        <span aria-hidden="true">&times;</span>
      </button>
      <div class="row centered">
        <div class="col-md-12">
          <div class="error-template">
            <h1>
              {{ title }}
            </h1>
            <div class="error-details">
              {{ details }}
            </div>
            <div class="error-actions">
              <a routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> Take me home
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['error.component.scss'],
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
