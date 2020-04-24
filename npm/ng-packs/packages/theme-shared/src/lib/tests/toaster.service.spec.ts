import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { NgxsModule } from '@ngxs/store';
import { timer } from 'rxjs';
import { ToastContainerComponent } from '../components/toast-container/toast-container.component';
import { ToastComponent } from '../components/toast/toast.component';
import { ToasterService } from '../services/toaster.service';

@NgModule({
  exports: [ToastContainerComponent],
  entryComponents: [ToastContainerComponent],
  declarations: [ToastContainerComponent, ToastComponent],
  imports: [CoreModule.forTest()],
})
export class MockModule {}

describe('ToasterService', () => {
  let spectator: SpectatorService<ToasterService>;
  let service: ToasterService;
  const createService = createServiceFactory({
    service: ToasterService,
    imports: [NgxsModule.forRoot(), CoreModule.forTest(), MockModule],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  afterEach(() => {
    clearElements();
  });

  test('should display a toast', async () => {
    service.show('MESSAGE', 'TITLE');

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    expect(selectToasterElement('.fa-exclamation-circle')).toBeTruthy();
    expect(selectToasterContent('.toast-title')).toBe('TITLE');
    expect(selectToasterContent('.toast-message')).toBe('MESSAGE');
  });

  test.each`
    type         | selector            | icon
    ${'info'}    | ${'.toast-info'}    | ${'.fa-info-circle'}
    ${'success'} | ${'.toast-success'} | ${'.fa-check-circle'}
    ${'warn'}    | ${'.toast-warning'} | ${'.fa-exclamation-triangle'}
    ${'error'}   | ${'.toast-error'}   | ${'.fa-times-circle'}
  `('should display $type toast', async ({ type, selector, icon }) => {
    service[type]('MESSAGE', 'TITLE');

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    expect(selectToasterContent('.toast-title')).toBe('TITLE');
    expect(selectToasterContent('.toast-message')).toBe('MESSAGE');
    expect(selectToasterElement()).toBe(document.querySelector(selector));
    expect(selectToasterElement(icon)).toBeTruthy();
  });

  test('should display multiple toasts', async () => {
    service.show('MESSAGE_1', 'TITLE_1');
    service.show('MESSAGE_2', 'TITLE_2');

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    const titles = document.querySelectorAll('.toast-title');
    expect(titles.length).toBe(2);

    const messages = document.querySelectorAll('.toast-message');
    expect(messages.length).toBe(2);
  });

  test('should remove a toast when remove is called', async () => {
    service.show('MESSAGE');
    service.remove(0);

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    expect(selectToasterElement()).toBeNull();
  });

  test('should remove toasts when clear is called', async () => {
    service.show('MESSAGE');
    service.clear();

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    expect(selectToasterElement()).toBeNull();
  });

  test('should remove toasts based on containerKey when clear is called with key', async () => {
    service.show('MESSAGE_1', 'TITLE_1', 'neutral', { containerKey: 'x' });
    service.show('MESSAGE_2', 'TITLE_2', 'neutral', { containerKey: 'y' });
    service.clear('x');

    await timer(0).toPromise();
    service['containerComponentRef'].changeDetectorRef.detectChanges();

    expect(selectToasterElement('.fa-exclamation-circle')).toBeTruthy();
    expect(selectToasterContent('.toast-title')).toBe('TITLE_2');
    expect(selectToasterContent('.toast-message')).toBe('MESSAGE_2');
  });
});

function clearElements(selector = '.toast') {
  document.querySelectorAll(selector).forEach(element => element.parentNode.removeChild(element));
}

function selectToasterContent(selector = '.toast'): string {
  return selectToasterElement(selector).textContent.trim();
}

function selectToasterElement<T extends HTMLElement>(selector = '.toast'): T {
  return document.querySelector(selector);
}
