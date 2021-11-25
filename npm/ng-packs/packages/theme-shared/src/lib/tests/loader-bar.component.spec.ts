import { HttpWaitService, LOADER_DELAY, SubscriptionService } from '@abp/ng.core';
import { HttpRequest } from '@angular/common/http';
import { NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { createComponentFactory, Spectator, SpyObject } from '@ngneat/spectator/jest';
import { Subject, timer } from 'rxjs';
import { LoaderBarComponent } from '../components/loader-bar/loader-bar.component';

describe('LoaderBarComponent', () => {
  let spectator: Spectator<LoaderBarComponent>;
  let router: SpyObject<Router>;
  const events$ = new Subject();

  const createComponent = createComponentFactory({
    component: LoaderBarComponent,
    detectChanges: false,
    providers: [
      SubscriptionService,
      { provide: Router, useValue: { events: events$ } },
      { provide: LOADER_DELAY, useValue: 0 },
    ],
  });

  beforeEach(() => {
    spectator = createComponent({});
    spectator.component.intervalPeriod = 1;
    spectator.component.stopDelay = 1;
    router = spectator.inject(Router);
  });

  it('should initial variable values are correct', () => {
    expect(spectator.component.containerClass).toBe('abp-loader-bar');
    expect(spectator.component.color).toBe('#77b6ff');
  });

  it('should increase the progressLevel', done => {
    spectator.detectChanges();
    const httpWaitService = spectator.inject(HttpWaitService);
    httpWaitService.addRequest(new HttpRequest('GET', 'test'));
    spectator.detectChanges();
    setTimeout(() => {
      expect(spectator.component.progressLevel > 0).toBeTruthy();
      done();
    }, 10);
  });

  it('should be interval unsubscribed', done => {
    const request = new HttpRequest('GET', 'test');

    spectator.detectChanges();
    const httpWaitService = spectator.inject(HttpWaitService);
    httpWaitService.addRequest(request);
    expect(spectator.component.interval.closed).toBe(false);
    httpWaitService.deleteRequest(request);
    timer(400).subscribe(() => {
      expect(spectator.component.interval.closed).toBe(true);
      done();
    });
  });

  it('should start and stop the loading with navigation', done => {
    spectator.detectChanges();
    events$.next(new NavigationStart(1, 'test'));
    expect(spectator.component.interval.closed).toBe(false);

    events$.next(new NavigationEnd(1, 'test', 'test'));
    events$.next(new NavigationError(1, 'test', 'test'));
    expect(spectator.component.progressLevel).toBe(100);

    timer(2).subscribe(() => {
      expect(spectator.component.progressLevel).toBe(0);
      done();
    });
  });

  it('should stop the loading with navigation', done => {
    spectator.detectChanges();
    events$.next(new NavigationStart(1, 'test'));
    expect(spectator.component.interval.closed).toBe(false);

    events$.next(new NavigationEnd(1, 'testend', 'testend'));
    expect(spectator.component.progressLevel).toBe(100);

    timer(2).subscribe(() => {
      expect(spectator.component.progressLevel).toBe(0);
      done();
    });
  });

  describe('#startLoading', () => {
    it('should return when isLoading is true', done => {
      spectator.detectChanges();
      events$.next(new NavigationStart(1, 'test'));
      events$.next(new NavigationStart(1, 'test'));
      done();
    });
  });
});
