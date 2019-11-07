import { LocalizationPipe } from '@abp/ng.core';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { ConfirmationComponent, ModalComponent, ButtonComponent } from '../components';

describe('ModalComponent', () => {
  let spectator: SpectatorHost<ModalComponent, { visible: boolean; busy: boolean; ngDirty: boolean }>;
  let appearFn;
  let disappearFn;
  const createHost = createHostFactory({
    component: ModalComponent,
    imports: [ToastModule],
    declarations: [ConfirmationComponent, LocalizationPipe, ButtonComponent],
    providers: [MessageService],
    mocks: [Store],
  });

  beforeEach(() => {
    appearFn = jest.fn(() => null);
    disappearFn = jest.fn(() => null);

    spectator = createHost(
      `<abp-modal [(visible)]="visible" [busy]="busy" [centered]="true" (appear)="appearFn()" (disappear)="disappearFn()" size="sm" modalClass="test">
        <ng-template #abpHeader>
          <div class="header"></div>
        </ng-template>

        <ng-template #abpBody>
          <div class="body">
            <input [class.ng-dirty]="ngDirty">
          </div>
        </ng-template>

        <ng-template #abpFooter>
         <div class="footer">
          <button id="abp-close" #abpClose></button>
          </div>
          </ng-template>
        <abp-button>Submit</abp-button>
        <abp-confirmation></abp-confirmation>
      </abp-modal>
      `,
      {
        hostProps: {
          visible: true,
          busy: false,
          ngDirty: true,
          appearFn,
          disappearFn,
        },
      },
    );
  });

  it('should be created', () => {
    expect(spectator.query('div.modal')).toBeTruthy();
    expect(spectator.query('div.modal-backdrop')).toBeTruthy();
    expect(spectator.query('div#abp-modal-dialog')).toBeTruthy();
  });

  it('should works right the inputs', () => {
    expect(spectator.query('div.test')).toBeTruthy();
    expect(spectator.query('div.modal-sm')).toBeTruthy();
    expect(spectator.query('div.modal-dialog-centered')).toBeTruthy();
  });

  it('should emit the appear output', () => {
    expect(appearFn).toHaveBeenCalled();
  });

  it('should emit the disappear output', () => {
    spectator.hostComponent.visible = false;
    spectator.detectChanges();
    expect(disappearFn).toHaveBeenCalled();
  });

  it('should open the confirmation popup and works correct', () => {
    spectator.click('#abp-modal-close-button');
    expect(disappearFn).not.toHaveBeenCalled();

    expect(spectator.query('p-toast')).toBeTruthy();
    spectator.click('button#cancel');
    expect(spectator.query('div.modal')).toBeTruthy();

    spectator.click('#abp-modal-close-button');
    spectator.click('button#confirm');
    expect(spectator.query('div.modal')).toBeFalsy();
    expect(disappearFn).toHaveBeenCalled();
  });

  it('should close with the abpClose', done => {
    spectator.hostComponent.ngDirty = false;
    spectator.detectChanges();
    setTimeout(() => {
      spectator.click('#abp-close');
      expect(disappearFn).toHaveBeenCalled();
      done();
    }, 10);
  });

  it('should close with esc key', done => {
    spectator.hostComponent.ngDirty = false;
    spectator.detectChanges();
    setTimeout(() => {
      spectator.dispatchKeyboardEvent(document.body, 'keyup', 'Escape');
    }, 0);
    setTimeout(() => {
      expect(spectator.component.visible).toBe(false);
      done();
    }, 200);
  });

  it('should not close when busy is true', done => {
    setTimeout(() => {
      spectator.hostComponent.busy = true;
      spectator.hostComponent.ngDirty = false;
      spectator.detectChanges();
      spectator.click('#abp-modal-close-button');
      expect(disappearFn).not.toHaveBeenCalled();
      expect(spectator.component.abpSubmit.loading).toBe(true);
      done();
    }, 0);
  });
});
