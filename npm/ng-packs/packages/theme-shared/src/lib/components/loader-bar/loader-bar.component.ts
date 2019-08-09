import { StartLoader, StopLoader } from '@abp/ng.core';
import { Component, Input, OnDestroy } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';
import { takeUntilDestroy } from '@ngx-validate/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
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
export class LoaderBarComponent implements OnDestroy {
  @Input()
  containerClass: string = 'abp-loader-bar';

  @Input()
  progressClass: string = 'abp-progress';

  @Input()
  isLoading: boolean = false;

  @Input()
  filter = (action: StartLoader | StopLoader) => action.payload.url.indexOf('openid-configuration') < 0;

  progressLevel: number = 0;

  interval: any;

  constructor(private actions: Actions, private router: Router) {
    actions
      .pipe(
        ofActionSuccessful(StartLoader, StopLoader),
        filter(this.filter),
        takeUntilDestroy(this),
      )
      .subscribe(action => {
        if (action instanceof StartLoader) this.startLoading();
        else this.stopLoading();
      });

    router.events
      .pipe(
        filter(event => event instanceof NavigationStart || event instanceof NavigationEnd),
        takeUntilDestroy(this),
      )
      .subscribe(event => {
        if (event instanceof NavigationStart) this.startLoading();
        else this.stopLoading();
      });
  }

  ngOnDestroy() {}

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
