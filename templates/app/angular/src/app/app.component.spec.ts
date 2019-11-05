import { LazyLoadService } from '@abp/ng.core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxsModule } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { AppComponent } from './app.component';
import { LoaderBarComponent } from '@abp/ng.theme.shared';
import { Subject, Observable } from 'rxjs';
import { Component } from '@angular/core';
import { By } from '@angular/platform-browser';

@Component({
  template: '',
  selector: 'abp-loader-bar',
})
class DummyLoaderBarComponent {}

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let mockLazyLoadService: { load: () => Observable<void> };
  let loadResponse$: Subject<void>;
  let spy: jasmine.Spy<() => Observable<void>>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AppComponent, DummyLoaderBarComponent],
      providers: [{ provide: LazyLoadService, useValue: { load: () => loadResponse$ } }],
    });
    loadResponse$ = new Subject();
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    mockLazyLoadService = TestBed.get(LazyLoadService);
    spy = spyOn(mockLazyLoadService, 'load');
    spy.and.returnValue(loadResponse$);
    fixture.detectChanges();
  });

  describe('LazyLoadService load method', () => {
    it('should call', () => {
      expect(spy).toHaveBeenCalledWith(
        [
          'primeng.min.css',
          'primeicons.css',
          'primeng-nova-light-theme.css',
          'fontawesome-all.min.css',
          'fontawesome-v4-shims.min.css',
        ],
        'style',
        null,
        'head',
      );
    });
  });

  describe('template', () => {
    it('should have the abp-loader-bar', () => {
      const abpLoader = fixture.debugElement.query(By.css('abp-loader-bar'));
      expect(abpLoader).toBeTruthy();
    });

    it('should have router-outlet', () => {
      const abpLoader = fixture.debugElement.query(By.css('router-outlet'));
      expect(abpLoader).toBeTruthy();
    });
  });
});
