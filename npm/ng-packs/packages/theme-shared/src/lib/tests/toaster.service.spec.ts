import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { ToasterService } from '../services/toaster.service';
import { ThemeSharedModule } from '../theme-shared.module';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'abp-dummy',
  template: `
    <abp-toast-container></abp-toast-container>
  `,
})
class DummyComponent {
  constructor(public toaster: ToasterService) {}
}

describe('ToasterService', () => {
  let spectator: Spectator<DummyComponent>;
  let service: ToasterService;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot(), RouterTestingModule],
    mocks: [OAuthService],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ToasterService);
  });

  test('should display an error toast', () => {
    service.error('test', 'title');

    spectator.detectChanges();

    expect(spectator.query('div.toast')).toBeTruthy();
    expect(spectator.query('.toast-icon i')).toHaveClass('fa-times-circle');
    expect(spectator.query('div.toast-title')).toHaveText('title');
    expect(spectator.query('p.toast-message')).toHaveText('test');
  });

  test('should display a warning toast', () => {
    service.warn('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('.toast-icon i')).toHaveClass('fa-exclamation-triangle');
  });

  test('should display a success toast', () => {
    service.success('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('.toast-icon i')).toHaveClass('fa-check-circle');
  });

  test('should display an info toast', () => {
    service.info('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('.toast-icon i')).toHaveClass('fa-info-circle');
  });

  test('should display multiple toasts', () => {
    service.info('detail1', 'summary1');
    service.info('detail2', 'summary2');

    spectator.detectChanges();
    expect(spectator.queryAll('div.toast-title').map(node => node.textContent.trim())).toEqual([
      'summary1',
      'summary2',
    ]);
    expect(spectator.queryAll('p.toast-message').map(node => node.textContent.trim())).toEqual([
      'detail1',
      'detail2',
    ]);
  });

  test('should remove the opened toasts', () => {
    service.info('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('div.toast')).toBeTruthy();

    service.clear();
    spectator.detectChanges();
    expect(spectator.query('p-div.toast')).toBeFalsy();
  });
});
