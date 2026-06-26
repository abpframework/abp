import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  input,
  OnDestroy,
  OnInit,
  signal,
} from '@angular/core';
import { combineLatest, Subscription, timer } from 'rxjs';
import { HttpWaitService, RouterWaitService, SubscriptionService } from '@abp/ng.core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-loader-bar',
  template: `
    <div id="abp-loader-bar" [class]="containerClass()" [class.is-loading]="isLoading()">
      <div
        class="abp-progress"
        [class.progressing]="progressLevel() > 0"
        [style.width.vw]="progressLevel()"
        [style]="{
          'background-color': color(),
          'box-shadow': boxShadow,
        }"
      ></div>
    </div>
  `,
  styleUrls: ['./loader-bar.component.scss'],
  providers: [SubscriptionService],
  imports: [],
})
export class LoaderBarComponent implements OnDestroy, OnInit {
  private subscription = inject(SubscriptionService);
  private httpWaitService = inject(HttpWaitService);
  private routerWaitService = inject(RouterWaitService);

  readonly isLoadingInput = input(false, { alias: 'isLoading' });
  readonly containerClass = input('abp-loader-bar');
  readonly color = input('#77b6ff');

  protected readonly isLoading = signal(false);
  protected readonly progressLevel = signal(0);

  interval = new Subscription();
  timer = new Subscription();
  intervalPeriod = 350;
  stopDelay = 800;

  constructor() {
    effect(() => {
      this.isLoading.set(this.isLoadingInput());
    });
  }

  private readonly clearProgress = () => {
    this.progressLevel.set(0);
  };

  private readonly reportProgress = () => {
    const current = this.progressLevel();
    if (current < 75) {
      this.progressLevel.set(current + 1 + Math.random() * 9);
    } else if (current < 90) {
      this.progressLevel.set(current + 0.4);
    } else if (current < 100) {
      this.progressLevel.set(current + 0.1);
    } else {
      this.interval.unsubscribe();
    }
  };

  get boxShadow(): string {
    return `0 0 10px rgba(${this.color()}, 0.5)`;
  }

  ngOnInit() {
    this.subscribeLoading();
  }

  subscribeLoading() {
    this.subscription.addOne(
      combineLatest([this.httpWaitService.getLoading$(), this.routerWaitService.getLoading$()]),
      ([httpLoading, routerLoading]) => {
        if (httpLoading || routerLoading) this.startLoading();
        else this.stopLoading();
      },
    );
  }

  ngOnDestroy() {
    this.interval.unsubscribe();
  }

  startLoading() {
    if (this.isLoading() || !this.interval.closed) return;

    this.isLoading.set(true);
    this.progressLevel.set(0);
    this.interval = timer(0, this.intervalPeriod).subscribe(this.reportProgress);
    this.timer.unsubscribe();
  }

  stopLoading() {
    this.interval.unsubscribe();

    this.progressLevel.set(100);
    this.isLoading.set(false);

    if (!this.timer.closed) return;

    this.timer = timer(this.stopDelay).subscribe(this.clearProgress);
  }
}
