import { CoreTestingModule } from '@abp/ng.core/testing';
import { NgModule } from '@angular/core';
import { fakeAsync, tick } from '@angular/core/testing';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { timer } from 'rxjs';
import { ConfirmationComponent } from '../components';
import { Confirmation } from '../models';
import { ConfirmationService } from '../services';

@NgModule({
  exports: [ConfirmationComponent],
  entryComponents: [ConfirmationComponent],
  declarations: [ConfirmationComponent],
  imports: [CoreTestingModule.withConfig()],
})
export class MockModule {}

describe('ConfirmationService', () => {
  let spectator: SpectatorService<ConfirmationService>;
  let service: ConfirmationService;
  const createService = createServiceFactory({
    service: ConfirmationService,
    imports: [CoreTestingModule.withConfig(), MockModule],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  afterEach(() => {
    clearElements();
  });

  test('should display a confirmation popup', fakeAsync(() => {
    service.show('MESSAGE', 'TITLE');

    tick();

    expect(selectConfirmationContent('.title')).toBe('TITLE');
    expect(selectConfirmationContent('.message')).toBe('MESSAGE');
  }));

  test('should display HTML string in title, message, and buttons', fakeAsync(() => {
    service.show(
      '<span class="custom-message">MESSAGE<span>',
      '<span class="custom-title">TITLE<span>',
      'neutral',
      {
        cancelText: '<span class="custom-cancel">CANCEL</span>',
        yesText: '<span class="custom-yes">YES</span>',
      },
    );

    tick();

    expect(selectConfirmationContent('.custom-title')).toBe('TITLE');
    expect(selectConfirmationContent('.custom-message')).toBe('MESSAGE');
    expect(selectConfirmationContent('.custom-cancel')).toBe('CANCEL');
    expect(selectConfirmationContent('.custom-yes')).toBe('YES');
  }));

  test('should display custom FA icon', fakeAsync(() => {
    service.show('MESSAGE', 'TITLE',undefined,{
      icon:'fa fa-info'
    });

    tick();
    expect(selectConfirmationElement(".icon").className).toBe('icon fa fa-info')
  }));

  test('should display custom icon as html element', fakeAsync(() => {
    const className = 'custom-icon'
    const selector = '.'+className;

    service.show('MESSAGE', 'TITLE',undefined,{
      iconTemplate: `<span class="${className}">I am icon</span>`
    });

    tick();

    const element = selectConfirmationElement(selector)
    expect(element).toBeTruthy()
    expect(element.innerHTML).toBe("I am icon")
  }));
  test.each`
    type         | selector      | icon
    ${'info'}    | ${'.info'}    | ${'.fa-info-circle'}
    ${'success'} | ${'.success'} | ${'.fa-check-circle'}
    ${'warn'}    | ${'.warning'} | ${'.fa-exclamation-triangle'}
    ${'error'}   | ${'.error'}   | ${'.fa-times-circle'}
  `('should display $type confirmation popup', async ({ type, selector, icon }) => {
    service[type]('MESSAGE', 'TITLE');

    await timer(0).toPromise();

    expect(selectConfirmationContent('.title')).toBe('TITLE');
    expect(selectConfirmationContent('.message')).toBe('MESSAGE');
    expect(selectConfirmationElement(selector)).toBeTruthy();
    expect(selectConfirmationElement(icon)).toBeTruthy();
  });

  // test('should close with ESC key', (done) => {
  //   service
  //     .info('', '')
  //     .pipe(take(1))
  //     .subscribe((status) => {
  //       expect(status).toBe(Confirmation.Status.dismiss);
  //       done();
  //     });

  //   const escape = new KeyboardEvent('keyup', { key: 'Escape' });
  //   document.dispatchEvent(escape);
  // });

  test('should close when click cancel button', done => {
    service.info('', '', { yesText: 'Sure', cancelText: 'Exit' }).subscribe(status => {
      expect(status).toBe(Confirmation.Status.reject);
      done();
    });

    timer(0).subscribe(() => {
      expect(selectConfirmationContent('button#cancel')).toBe('Exit');
      expect(selectConfirmationContent('button#confirm')).toBe('Sure');

      (document.querySelector('button#cancel') as HTMLButtonElement).click();
    });
  });

  test.each`
    dismissible | count
    ${true}     | ${1}
    ${false}    | ${0}
  `(
    'should call the listenToEscape method $count times when dismissible is $dismissible',
    ({ dismissible, count }) => {
      const spy = jest.spyOn(service as any, 'listenToEscape');

      service.info('', '', { dismissible });

      expect(spy).toHaveBeenCalledTimes(count);
    },
  );
});

function clearElements(selector = '.confirmation') {
  document.querySelectorAll(selector).forEach(element => element.parentNode.removeChild(element));
}

function selectConfirmationContent(selector = '.confirmation'): string {
  return selectConfirmationElement(selector).textContent.trim();
}

function selectConfirmationElement<T extends HTMLElement>(selector = '.confirmation'): T {
  return document.querySelector(selector);
}
