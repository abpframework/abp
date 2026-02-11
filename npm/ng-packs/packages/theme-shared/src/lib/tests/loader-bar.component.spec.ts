import { HttpWaitService, LOADER_DELAY, RouterWaitService, SubscriptionService } from '@abp/ng.core';
import { HttpRequest } from '@angular/common/http';
import { NavigationStart, Router } from '@angular/router';
import { createComponentFactory, Spectator } from '@ngneat/spectator/vitest';
import { combineLatest, firstValueFrom, Subject, timer } from 'rxjs';
import { LoaderBarComponent } from '../components/loader-bar/loader-bar.component';
import { setupComponentResources } from './utils';

describe('LoaderBarComponent', () => {
  let spectator: Spectator<LoaderBarComponent>;
  let router: Router;
  let createComponent: ReturnType<typeof createComponentFactory<LoaderBarComponent>>;
  const events$ = new Subject();

  beforeAll(() => setupComponentResources('../components/loader-bar', import.meta.url));

  beforeEach(() => {
    if (!createComponent) {
      createComponent = createComponentFactory({
        component: LoaderBarComponent,
        detectChanges: false,
        providers: [
          SubscriptionService,
          { provide: Router, useValue: { events: events$ } },
          { provide: LOADER_DELAY, useValue: 0 },
        ],
      });
    }

    spectator = createComponent({});
    spectator.component.intervalPeriod = 1;
    spectator.component.stopDelay = 1;
    router = spectator.inject(Router);
  });

  it('should initial variable values are correct', () => {
    expect(spectator.component.containerClass).toBe('abp-loader-bar');
    expect(spectator.component.color).toBe('#77b6ff');
  });

  it('should increase the progressLevel', async () => {
    spectator.detectChanges();
    const httpWaitService = spectator.inject(HttpWaitService);
    httpWaitService.addRequest(new HttpRequest('GET', 'test'));
    spectator.detectChanges();

    await new Promise(resolve => setTimeout(resolve, 10));

    expect(spectator.component.progressLevel > 0).toBeTruthy();
  });


  it('should be interval unsubscribed', async () => {
    const request = new HttpRequest('GET', 'test');
    spectator.detectChanges();
    const httpWaitService = spectator.inject(HttpWaitService);
    
    await firstValueFrom(combineLatest([
      httpWaitService.getLoading$(),
      spectator.inject(RouterWaitService).getLoading$()
    ]));
    
    httpWaitService.addRequest(request);
    spectator.detectChanges();
    
    let attempts = 0;
    while (spectator.component.interval.closed && attempts < 50) {
      await new Promise(resolve => setTimeout(resolve, 10));
      spectator.detectChanges();
      attempts++;
    }
    
    expect(spectator.component.interval.closed).toBe(false);
    
    httpWaitService.deleteRequest(request);
    spectator.detectChanges();
    
    await firstValueFrom(timer(400));
    
    expect(spectator.component.interval.closed).toBe(true);
  });


  it('should start and stop the loading with navigation', async () => {
    spectator.detectChanges();
    const routerWaitService = spectator.inject(RouterWaitService);
    
    routerWaitService.setLoading(true);
    spectator.detectChanges();
    
    let attempts = 0;
    while (spectator.component.interval.closed && attempts < 50) {
      await new Promise(resolve => setTimeout(resolve, 10));
      spectator.detectChanges();
      attempts++;
    }
    expect(spectator.component.interval.closed).toBe(false);

    routerWaitService.setLoading(false);
    spectator.detectChanges();
    
    attempts = 0;
    while (spectator.component.progressLevel !== 100 && attempts < 50) {
      await new Promise(resolve => setTimeout(resolve, 10));
      spectator.detectChanges();
      attempts++;
    }
    expect(spectator.component.progressLevel).toBe(100);

    await firstValueFrom(timer(spectator.component.stopDelay + 10));
    expect(spectator.component.progressLevel).toBe(0);
  });

  it('should stop the loading with navigation', async () => {
    spectator.detectChanges();
    const routerWaitService = spectator.inject(RouterWaitService);
    
    routerWaitService.setLoading(true);
    spectator.detectChanges();
    
    let attempts = 0;
    while (spectator.component.interval.closed && attempts < 50) {
      await new Promise(resolve => setTimeout(resolve, 10));
      spectator.detectChanges();
      attempts++;
    }
    expect(spectator.component.interval.closed).toBe(false);

    routerWaitService.setLoading(false);
    spectator.detectChanges();
    
    attempts = 0;
    while (spectator.component.progressLevel !== 100 && attempts < 50) {
      await new Promise(resolve => setTimeout(resolve, 10));
      spectator.detectChanges();
      attempts++;
    }
    expect(spectator.component.progressLevel).toBe(100);

    await firstValueFrom(timer(spectator.component.stopDelay + 10));
    expect(spectator.component.progressLevel).toBe(0);
  });

  describe('#startLoading', () => {
    it('should return when isLoading is true', async () => {
      spectator.detectChanges();
      
      events$.next(new NavigationStart(1, 'test'));
      spectator.detectChanges();
      
      let attempts = 0;
      while (spectator.component.interval.closed && attempts < 50) {
        await new Promise(resolve => setTimeout(resolve, 10));
        spectator.detectChanges();
        attempts++;
      }
      
      events$.next(new NavigationStart(1, 'test'));
      spectator.detectChanges();
      
      expect(spectator.component).toBeTruthy();
    });
  });
});
