import { NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { createHostFactory, SpectatorHost, SpyObject } from '@ngneat/spectator/jest';
import { Subject, Subscription, timer } from 'rxjs';
import { LoaderBarComponent } from '../components/loader-bar/loader-bar.component';
import { SubscriptionService } from '@abp/ng.core';
import { HttpRequest } from '@angular/common/http';
import { HttpWaitService } from '../../../../core/src/lib/services';

describe('LoaderBarComponent', () => {
  let spectator: SpectatorHost<LoaderBarComponent>;
  let router: SpyObject<Router>;
  const events$ = new Subject();

  const createHost = createHostFactory({
    component: LoaderBarComponent,
    mocks: [Router],
    detectChanges: false,
    providers: [SubscriptionService],
  });

  beforeEach(() => {
    spectator = createHost('<abp-loader-bar></abp-loader-bar>');
    spectator.component.intervalPeriod = 1;
    spectator.component.stopDelay = 1;
    router = spectator.inject(Router);
    (router as any).events = events$;
  });

  it('should initial variable values are correct', () => {
    spectator.component.interval = new Subscription();
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

  test.skip('should be interval unsubscribed', done => {
    spectator.detectChanges();
    const httpWaitService = spectator.inject(HttpWaitService);
    httpWaitService.addRequest(new HttpRequest('GET', 'test'));
    expect(spectator.component.interval.closed).toBe(false);

    timer(400).subscribe(() => {
      expect(spectator.component.interval.closed).toBe(true);
      done();
    });
  });

  it('should start and stop the loading with navigation', done => {
    spectator.detectChanges();
    (router as any).events.next(new NavigationStart(1, 'test'));
    expect(spectator.component.interval.closed).toBe(false);

    (router as any).events.next(new NavigationEnd(1, 'test', 'test'));
    (router as any).events.next(new NavigationError(1, 'test', 'test'));
    expect(spectator.component.progressLevel).toBe(100);

    timer(2).subscribe(() => {
      expect(spectator.component.progressLevel).toBe(0);
      done();
    });
  });

  it('should stop the loading with navigation', done => {
    spectator.detectChanges();
    (router as any).events.next(new NavigationStart(1, 'test'));
    expect(spectator.component.interval.closed).toBe(false);

    (router as any).events.next(new NavigationEnd(1, 'testend', 'testend'));
    expect(spectator.component.progressLevel).toBe(100);

    timer(2).subscribe(() => {
      expect(spectator.component.progressLevel).toBe(0);
      done();
    });
  });

  describe('#startLoading', () => {
    it('should return when isLoading is true', done => {
      spectator.detectChanges();
      (router as any).events.next(new NavigationStart(1, 'test'));
      (router as any).events.next(new NavigationStart(1, 'test'));
      done();
    });
  });
});
