import { LocalizationPipe } from '@abp/ng.core';
import { RouterTestingModule } from '@angular/router/testing';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { fromEvent, Subject, timer } from 'rxjs';
import { delay, reduce, take } from 'rxjs/operators';
import { ButtonComponent, ConfirmationComponent, ModalComponent } from '../components';
import { ModalContainerComponent } from '../components/modal/modal-container.component';
import { Confirmation } from '../models';
import { ConfirmationService, ModalService } from '../services';

describe('ModalComponent', () => {
  let spectator: SpectatorHost<
    ModalComponent,
    { visible: boolean; busy: boolean; ngDirty: boolean }
  >;
  let appearFn;
  let disappearFn;
  let mockConfirmation$: Subject<Confirmation.Status>;
  const createHost = createHostFactory({
    component: ModalComponent,
    imports: [RouterTestingModule],
    declarations: [
      ConfirmationComponent,
      LocalizationPipe,
      ButtonComponent,
      ModalContainerComponent,
    ],
    entryComponents: [ModalContainerComponent],
    providers: [
      {
        provide: ConfirmationService,
        useValue: {
          warn() {
            mockConfirmation$ = new Subject();
            return mockConfirmation$;
          },
        },
      },
    ],
    mocks: [Store],
  });

  beforeEach(async () => {
    appearFn = jest.fn();
    disappearFn = jest.fn();

    spectator = createHost(
      `<abp-modal [(visible)]="visible" [busy]="busy" [centered]="true" (appear)="appearFn()" (disappear)="disappearFn()" size="sm" modalClass="test">
        <ng-template #abpHeader>
          <div class="header"></div>
        </ng-template>

        <ng-template #abpBody>
          <div class="body"><input [class.ng-dirty]="ngDirty"></div>
        </ng-template>

        <ng-template #abpFooter>
          <div class="footer">
            <button id="abp-close" #abpClose></button>
            <abp-button>Submit</abp-button>
          </div>
        </ng-template>
      </abp-modal>
      `,
      {
        hostProps: {
          visible: true,
          busy: false,
          ngDirty: false,
          appearFn,
          disappearFn,
        },
      },
    );

    await wait0ms();
  });

  afterEach(() => {
    const modalService = spectator.get(ModalService);
    modalService.clearModal();
  });

  it('should project its template to abp-modal-container', () => {
    const modal = selectModal();
    expect(modal).toBeTruthy();
    expect(modal.querySelector('div.modal-backdrop')).toBeTruthy();
    expect(modal.querySelector('div#abp-modal-dialog')).toBeTruthy();
  });

  it('should reflect its input properties to the template', () => {
    const modal = selectModal('.test');
    expect(modal).toBeTruthy();
    expect(modal.querySelector('div.modal-sm')).toBeTruthy();
    expect(modal.querySelector('div.modal-dialog-centered')).toBeTruthy();
  });

  it('should emit the appear output when made visible', () => {
    expect(appearFn).toHaveBeenCalled();
  });

  it('should emit the disappear output when made invisible', async () => {
    spectator.hostComponent.visible = false;
    spectator.detectChanges();

    await wait0ms();

    expect(disappearFn).toHaveBeenCalledTimes(1);
  });

  xit('should close with the abpClose', async () => {
    await wait0ms();

    spectator.dispatchMouseEvent(spectator.component.abpClose, 'click');

    await wait0ms();

    expect(disappearFn).toHaveBeenCalledTimes(1);
  });

  it('should open the confirmation popup and works correct', async () => {
    const confirmationService = spectator.get(ConfirmationService);
    const warnSpy = jest.spyOn(confirmationService, 'warn');

    await wait0ms();

    spectator.hostComponent.ngDirty = true;
    spectator.detectChanges();

    expect(selectModal()).toBeTruthy();
    spectator.component.close(); // 1st try

    await wait0ms();

    spectator.component.close(); // 2nd try

    await wait0ms();

    expect(selectModal()).toBeTruthy();
    expect(warnSpy).toHaveBeenCalledTimes(1);
    warnSpy.mockClear();

    mockConfirmation$.next(Confirmation.Status.reject);

    await wait0ms();

    expect(selectModal()).toBeTruthy();
    spectator.component.close();

    await wait0ms();

    expect(selectModal()).toBeTruthy();
    expect(warnSpy).toHaveBeenCalledTimes(1);
    warnSpy.mockClear();

    mockConfirmation$.next(Confirmation.Status.confirm);

    await wait0ms();

    expect(selectModal()).toBeNull();
    expect(disappearFn).toHaveBeenCalledTimes(1);
  });

  it('should close with esc key', async () => {
    await wait0ms();

    spectator.dispatchKeyboardEvent(document.body, 'keyup', 'Escape');

    await wait300ms();

    expect(spectator.component.visible).toBe(false);
  });

  it('should not close when busy is true', async () => {
    spectator.hostComponent.busy = true;
    spectator.detectChanges();

    spectator.component.close();

    await wait0ms();

    expect(disappearFn).not.toHaveBeenCalled();
  });

  it('should not let window unload when form is dirty', async done => {
    fromEvent(window, 'beforeunload')
      .pipe(
        take(2),
        delay(0),
        reduce<Event[]>((acc, v) => acc.concat(v), []),
      )
      .subscribe(([event1, event2]) => {
        expect(event1.returnValue).toBe(true);
        expect(event2.returnValue).toBe(false);
        done();
      });

    spectator.hostComponent.ngDirty = true;
    spectator.detectChanges();
    spectator.dispatchFakeEvent(window, 'beforeunload');

    await wait0ms();

    spectator.hostComponent.ngDirty = false;
    spectator.detectChanges();
    spectator.dispatchFakeEvent(window, 'beforeunload');
  });
});

function selectModal(modalSelector = ''): Element {
  return document.querySelector(`abp-modal-container div.modal${modalSelector}`);
}

async function wait0ms() {
  await timer(0).toPromise();
}

async function wait300ms() {
  await timer(300).toPromise();
}
