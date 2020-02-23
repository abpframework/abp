import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
import { ThemeSharedModule } from '../theme-shared.module';
import { OAuthModule, OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'abp-dummy',
  template: `
    <abp-confirmation></abp-confirmation>
  `,
})
class DummyComponent {
  constructor(public confirmation: ConfirmationService) {}
}

describe('ConfirmationService', () => {
  let spectator: Spectator<DummyComponent>;
  let service: ConfirmationService;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot(), RouterTestingModule],
    mocks: [OAuthService],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ConfirmationService);
  });

  test('should display a confirmation popup', () => {
    service.info('test', 'title');

    spectator.detectChanges();

    expect(spectator.query('div.confirmation .title')).toHaveText('title');
    expect(spectator.query('div.confirmation .message')).toHaveText('test');
  });

  test('should close with ESC key', done => {
    service.info('test', 'title').subscribe(() => {
      setTimeout(() => {
        spectator.detectComponentChanges();
        expect(spectator.query('div.confirmation')).toBeFalsy();
        done();
      }, 0);
    });

    spectator.detectChanges();
    expect(spectator.query('div.confirmation')).toBeTruthy();
    spectator.dispatchKeyboardEvent('div.confirmation', 'keyup', 'Escape');
  });

  test('should close when click cancel button', done => {
    service.info('test', 'title', { yesText: 'Sure', cancelText: 'Exit' }).subscribe(() => {
      spectator.detectComponentChanges();
      setTimeout(() => {
        expect(spectator.query('div.confirmation')).toBeFalsy();
        done();
      }, 0);
    });

    spectator.detectChanges();

    expect(spectator.query('div.confirmation')).toBeTruthy();
    expect(spectator.query('button#cancel')).toHaveText('Exit');
    expect(spectator.query('button#confirm')).toHaveText('Sure');

    spectator.click('button#cancel');
  });
});
