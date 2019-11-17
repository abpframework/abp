import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { MessageService } from 'primeng/components/common/messageservice';
import { ConfirmationService } from '../services/confirmation.service';
import { ThemeSharedModule } from '../theme-shared.module';

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
    imports: [CoreModule, ThemeSharedModule, NgxsModule.forRoot(), RouterTestingModule],
    providers: [MessageService],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ConfirmationService);
  });

  it('should display a confirmation popup', () => {
    service.info('test', 'title');

    spectator.detectChanges();

    expect(spectator.query('p-toast')).toBeTruthy();
    expect(spectator.query('p-toastitem')).toBeTruthy();
    expect(spectator.query('div.abp-confirm-summary')).toHaveText('title');
    expect(spectator.query('div.abp-confirm-body')).toHaveText('test');
  });

  it('should close with ESC key', done => {
    service.info('test', 'title');
    spectator.detectChanges();

    expect(spectator.query('p-toastitem')).toBeTruthy();

    spectator.dispatchKeyboardEvent('abp-confirmation', 'keyup', 'Escape');
    service.destroy$.subscribe(() => {
      // expect(spectator.query('p-toastitem')).toBeFalsy();
      spectator.detectComponentChanges();
      expect(spectator.query('p-toastitem')).toBeFalsy();
      done();
    });
  });

  it('should close when click cancel button', done => {
    service.info('test', 'title', { yesText: 'Sure', cancelText: 'Exit' });
    spectator.detectChanges();

    expect(spectator.query('p-toastitem')).toBeTruthy();
    expect(spectator.query('button#cancel')).toHaveText('Exit');
    expect(spectator.query('button#confirm')).toHaveText('Sure');

    service.status$.subscribe(() => {
      spectator.detectComponentChanges();
      expect(spectator.query('p-toastitem')).toBeFalsy();
      done();
    });
    spectator.click('button#cancel');
  });
});
