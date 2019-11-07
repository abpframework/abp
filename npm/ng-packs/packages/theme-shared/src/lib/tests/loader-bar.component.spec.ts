import { Router, RouteReuseStrategy, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
import { createHostFactory, SpectatorHost, SpyObject } from '@ngneat/spectator/jest';
import { Actions, NgxsModule, Store } from '@ngxs/store';
import { Subject, Subscription, Observable, Subscriber, timer } from 'rxjs';
import { LoaderBarComponent } from '../components/loader-bar/loader-bar.component';
import { StartLoader, StopLoader } from '@abp/ng.core';
import { HttpRequest } from '@angular/common/http';

describe('LoaderBarComponent', () => {
  let spectator: SpectatorHost<LoaderBarComponent>;
  let router: SpyObject<Router>;
  const events$ = new Subject();

  const createHost = createHostFactory({
    component: LoaderBarComponent,
    mocks: [Router],
    imports: [NgxsModule.forRoot()],
    detectChanges: false,
  });

  beforeEach(() => {
    spectator = createHost('<abp-loader-bar></abp-loader-bar>');
    spectator.component.intervalPeriod = 1;
    spectator.component.stopDelay = 1;
    router = spectator.get(Router);
    (router as any).events = events$;
  });

  it('should initial variable values are correct', () => {
    spectator.component.interval = new Subscription();
    expect(spectator.component.containerClass).toBe('abp-loader-bar');
    expect(spectator.component.color).toBe('#77b6ff');
  });

  it('should increase the progressLevel maximum 10 point when value is 0', done => {
    spectator.detectChanges();
    spectator.get(Store).dispatch(new StartLoader(new HttpRequest('GET', 'test')));
    setTimeout(() => {
      expect(spectator.component.progressLevel > 0 && spectator.component.progressLevel < 10).toBeTruthy();
      done();
    }, 2);
  });

  it('should be interval unsubscribed', done => {
    spectator.detectChanges();
    spectator.get(Store).dispatch(new StartLoader(new HttpRequest('GET', 'test')));
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

    spectator.get(Store).dispatch(new StopLoader(new HttpRequest('GET', 'test')));
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
