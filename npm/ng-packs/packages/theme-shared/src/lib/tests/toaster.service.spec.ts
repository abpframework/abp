import { CoreModule } from '@abp/ng.core';
import { Component } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { ToasterService } from '../services/toaster.service';
import { ThemeSharedModule } from '../theme-shared.module';
import { RouterModule } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'abp-dummy',
  template: `
    <abp-toast></abp-toast>
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
    imports: [CoreModule, ThemeSharedModule, NgxsModule.forRoot(), RouterTestingModule],
    providers: [MessageService],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.get(ToasterService);
  });

  it('should display an error toast', () => {
    service.error('test', 'title');

    spectator.detectChanges();

    expect(spectator.query('p-toast')).toBeTruthy();
    expect(spectator.query('p-toastitem')).toBeTruthy();
    expect(spectator.query('span.ui-toast-icon')).toHaveClass('pi-times');
    expect(spectator.query('div.ui-toast-summary')).toHaveText('title');
    expect(spectator.query('div.ui-toast-detail')).toHaveText('test');
  });

  it('should display a warning toast', () => {
    service.warn('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('span.ui-toast-icon')).toHaveClass('pi-exclamation-triangle');
  });

  it('should display a success toast', () => {
    service.success('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('span.ui-toast-icon')).toHaveClass('pi-check');
  });

  it('should display an info toast', () => {
    service.info('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('span.ui-toast-icon')).toHaveClass('pi-info-circle');
  });

  it('should display multiple toasts', () => {
    service.addAll([{ summary: 'summary1', detail: 'detail1' }, { summary: 'summary2', detail: 'detail2' }]);
    spectator.detectChanges();
    expect(spectator.queryAll('div.ui-toast-summary').map(node => node.textContent.trim())).toEqual([
      'summary1',
      'summary2',
    ]);
    expect(spectator.queryAll('div.ui-toast-detail').map(node => node.textContent.trim())).toEqual([
      'detail1',
      'detail2',
    ]);
  });

  it('should remove the opened toast', () => {
    service.info('test', 'title');
    spectator.detectChanges();
    expect(spectator.query('p-toastitem')).toBeTruthy();

    service.clear();
    spectator.detectChanges();
    expect(spectator.query('p-toastitem')).toBeFalsy();
  });
});
