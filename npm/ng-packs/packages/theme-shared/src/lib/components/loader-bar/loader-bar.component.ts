import { Component, OnInit, Input } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { LoaderStart, LoaderStop } from '@abp/ng.core';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'abp-loader-bar',
  template: `
    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">
      <div [ngClass]="progressClass" [style.width.vw]="progressLevel"></div>
    </div>
  `,
  styleUrls: ['./loader-bar.component.scss'],
})
export class LoaderBarComponent {
  @Input()
  containerClass: string = 'abp-loader-bar';

  @Input()
  progressClass: string = 'abp-progress';

  @Input()
  isLoading: boolean = false;

  @Input()
  filter = (action: LoaderStart | LoaderStop) => action.payload.url.indexOf('openid-configuration') < 0;

  progressLevel: number = 0;

  interval: any;

  constructor(private actions: Actions) {
    actions
      .pipe(
        ofActionSuccessful(LoaderStart, LoaderStop),
        filter(this.filter),
      )
      .subscribe(action => {
        if (action instanceof LoaderStart) {
          this.startLoading();
        } else {
          this.stopLoading();
        }
      });
  }

  startLoading() {
    this.isLoading = true;
    const interval = setInterval(() => {
      if (this.progressLevel < 75) {
        this.progressLevel += Math.random() * 10;
      } else if (this.progressLevel < 90) {
        this.progressLevel += 0.4;
      } else if (this.progressLevel < 100) {
        this.progressLevel += 0.1;
      } else {
        clearInterval(interval);
      }
    }, 300);

    this.interval = interval;
  }

  stopLoading() {
    clearInterval(this.interval);
    this.progressLevel = 100;
    this.isLoading = false;

    setTimeout(() => {
      this.progressLevel = 0;
    }, 800);
  }
}
