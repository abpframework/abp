import { LoaderStart, LoaderStop } from '@abp/ng.core';
import { Component, Input, OnInit } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'abp-button',
  template: `
    <button [attr.type]="buttonType" [ngClass]="buttonClass" [disabled]="loading">
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent implements OnInit {
  @Input()
  buttonClass: string = 'btn btn-primary';

  @Input()
  buttonType: string = 'button';

  @Input()
  iconClass: string;

  @Input()
  loading: boolean = false;

  @Input()
  requestType: string | string[];

  @Input()
  requestURLContainSearchValue: string;

  get icon(): string {
    return `${this.loading ? 'fa fa-spin fa-spinner' : this.iconClass || 'd-none'}`;
  }

  constructor(private actions: Actions) {}

  ngOnInit(): void {
    if (this.requestType || this.requestURLContainSearchValue) {
      this.actions
        .pipe(
          ofActionSuccessful(LoaderStart, LoaderStop),
          filter((event: LoaderStart | LoaderStop) => {
            let condition = true;
            if (this.requestType) {
              if (!Array.isArray(this.requestType)) this.requestType = [this.requestType];

              condition =
                condition &&
                this.requestType.findIndex(type => type.toLowerCase() === event.payload.method.toLowerCase()) > -1;
            }

            if (condition && this.requestURLContainSearchValue) {
              condition =
                condition &&
                event.payload.url.toLowerCase().indexOf(this.requestURLContainSearchValue.toLowerCase()) > -1;
            }

            return condition;
          }),
        )
        .subscribe(() => {
          this.loading = !this.loading;
        });
    }
  }
}
