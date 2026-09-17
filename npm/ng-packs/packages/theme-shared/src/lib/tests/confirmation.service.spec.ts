import { CoreTestingModule } from '@abp/ng.core/testing';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { firstValueFrom, timer } from 'rxjs';
import { ConfirmationComponent } from '../components';
import { Confirmation } from '../models';
import { ConfirmationService } from '../services';
import { CONFIRMATION_ICONS, DEFAULT_CONFIRMATION_ICONS } from '../tokens/confirmation-icons.token';
import { setupComponentResources } from './utils';

describe('ConfirmationService', () => {
  let spectator: SpectatorService<ConfirmationService>;
  let service: ConfirmationService;
  
  const createService = createServiceFactory({
    service: ConfirmationService,
    imports: [CoreTestingModule.withConfig(), ConfirmationComponent],
    providers: [{ provide: CONFIRMATION_ICONS, useValue: DEFAULT_CONFIRMATION_ICONS }],
  });

  beforeAll(async () => {
    await setupComponentResources('../components/confirmation', import.meta.url);
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  afterEach(() => {
    clearElements();
  });

  test('should display a confirmation popup', async () => {
    service.show('_::MESSAGE', '_::TITLE');

    await firstValueFrom(timer(10));

    expect(selectConfirmationContent('.title')).toBe('TITLE');
    expect(selectConfirmationContent('.message')).toBe('MESSAGE');
  });

  test('should display HTML string in title, message, and buttons', async () => {
    service.show(
      '_::<span class="custom-message">MESSAGE<span>',
      '_::<span class="custom-title">TITLE<span>',
      'neutral',
      {
        cancelText: '_::<span class="custom-cancel">CANCEL</span>',
        yesText: '_::<span class="custom-yes">YES</span>',
      },
    );

    await firstValueFrom(timer(10));

    expect(selectConfirmationContent('.custom-title')).toBe('TITLE');
    expect(selectConfirmationContent('.custom-message')).toBe('MESSAGE');
    expect(selectConfirmationContent('.custom-cancel')).toBe('CANCEL');
    expect(selectConfirmationContent('.custom-yes')).toBe('YES');
  });

  test('should display custom FA icon', async () => {
    service.show('_::MESSAGE', '_::TITLE', undefined, {
      icon: 'fa fa-info',
    });

    await firstValueFrom(timer(10));
    expect(selectConfirmationElement('.icon').className).toBe('icon fa fa-info');
  });

  test('should display custom icon as html element', async () => {
    const className = 'custom-icon';
    const selector = '.' + className;

    service.show('_::MESSAGE', '_::TITLE', undefined, {
      iconTemplate: `<span class="${className}">I am icon</span>`,
    });

    await firstValueFrom(timer(10));

    const element = selectConfirmationElement(selector);
    expect(element).toBeTruthy();
    expect(element.innerHTML).toBe('I am icon');
  });


  test.each`
    type         | selector      | icon
    ${'info'}    | ${'.info'}    | ${'.fa-info-circle'}
    ${'success'} | ${'.success'} | ${'.fa-check-circle'}
    ${'warn'}    | ${'.warning'} | ${'.fa-exclamation-triangle'}
    ${'error'}   | ${'.error'}   | ${'.fa-times-circle'}
  `('should display $type confirmation popup', async ({ type, selector, icon }) => {
    service[type]('_::MESSAGE', '_::TITLE');

    await firstValueFrom(timer(10));

    expect(selectConfirmationContent('.title')).toBe('TITLE');
    expect(selectConfirmationContent('.message')).toBe('MESSAGE');
    expect(selectConfirmationElement(selector)).toBeTruthy();
    expect(selectConfirmationElement(icon)).toBeTruthy();
  });


  test('should close when click cancel button', async () => {
    service.info('_::', '_::', { yesText: '_::Sure', cancelText: '_::Exit' }).subscribe(status => {
      expect(status).toBe(Confirmation.Status.reject);
    });

    await firstValueFrom(timer(10));

    expect(selectConfirmationContent('button#cancel')).toBe('Exit');
    expect(selectConfirmationContent('button#confirm')).toBe('Sure');

    (document.querySelector('button#cancel') as HTMLButtonElement).click();
  });

  test.each`
    dismissible | count
    ${true}     | ${1}
    ${false}    | ${0}
  `(
    'should call the listenToEscape method $count times when dismissible is $dismissible',
    ({ dismissible, count }) => {
      const spy = vi.spyOn(service as any, 'listenToEscape');

      service.info('_::', '_::', { dismissible });

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
