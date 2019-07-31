import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';

@Component({
  selector: 'abp-error-500',
  template: `
    <div class="error">
      <div class="row centered">
        <div class="col-md-12">
          <div class="error-template">
            <h1>
              Oops!
            </h1>
            <div class="error-details">
              Sorry, an error has occured.
            </div>
            <div class="error-actions">
              <a routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> Take Me Home
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['error-500.component.scss'],
})
export class Error500Component implements OnInit {
  constructor(private store: Store) {}

  ngOnInit(): void {}
}
