import { StartLoader, StopLoader } from '@abp/ng.core';
import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { takeUntilDestroy } from '@ngx-validate/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { Subscription, timer } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'abp-loader-bar',
  template: `
    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">
      <div
        class="abp-progress"
        [class.progressing]="progressLevel"
        [style.width.vw]="progressLevel"
        [ngStyle]="{
          'background-color': color,
          'box-shadow': boxShadow
        }"
      ></div>
    </div>
  `,
  styleUrls: ['./loader-bar.component.scss'],
})
export class LoaderBarComponent implements OnDestroy, OnInit {
  @Input()
  containerClass = 'abp-loader-bar';

  @Input()
  color = '#77b6ff';

  @Input()
  filter = (action: StartLoader | StopLoader) =>
    action.payload.url.indexOf('openid-configuration') < 0;

  @Input()
  isLoading = false;

  progressLevel = 0;

  interval: Subscription;

  timer: Subscription;

  intervalPeriod = 350;

  stopDelay = 800;

  private readonly clearProgress = () => {
    this.progressLevel = 0;
    this.cdRef.detectChanges();
  };

  private readonly reportProgress = () => {
    if (this.progressLevel < 75) {
      this.progressLevel += 1 + Math.random() * 9;
    } else if (this.progressLevel < 90) {
      this.progressLevel += 0.4;
    } else if (this.progressLevel < 100) {
      this.progressLevel += 0.1;
    } else {
      this.interval.unsubscribe();
    }
    this.cdRef.detectChanges();
  };

  get boxShadow(): string {
    return `0 0 10px rgba(${this.color}, 0.5)`;
  }

  constructor(private actions: Actions, private router: Router, private cdRef: ChangeDetectorRef) {}

  private subscribeToLoadActions() {
    this.actions
      .pipe(
        ofActionSuccessful(StartLoader, StopLoader),
        filter(this.filter),
        takeUntilDestroy(this),
      )
      .subscribe(action => {
        if (action instanceof StartLoader) this.startLoading();
        else this.stopLoading();
      });
  }

  private subscribeToRouterEvents() {
    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof NavigationStart ||
            event instanceof NavigationEnd ||
            event instanceof NavigationError,
        ),
        takeUntilDestroy(this),
      )
      .subscribe(event => {
        if (event instanceof NavigationStart) this.startLoading();
        else this.stopLoading();
      });
  }

  ngOnInit() {
    this.subscribeToLoadActions();
    this.subscribeToRouterEvents();
  }

  ngOnDestroy() {
    if (this.interval) this.interval.unsubscribe();
  }

  startLoading() {
    if (this.isLoading || (this.interval && !this.interval.closed)) return;

    this.isLoading = true;

    this.interval = timer(0, this.intervalPeriod).subscribe(this.reportProgress);
  }

  stopLoading() {
    if (this.interval) this.interval.unsubscribe();

    this.progressLevel = 100;
    this.isLoading = false;

    if (this.timer && !this.timer.closed) return;

    this.timer = timer(this.stopDelay).subscribe(this.clearProgress);
  }
}
