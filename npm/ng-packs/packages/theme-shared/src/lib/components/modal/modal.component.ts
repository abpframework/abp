import { SubscriptionService, uuid } from '@abp/ng.core';
import {
  Component,
  DestroyRef,
  OnDestroy,
  OnInit,
  TemplateRef,
  contentChild,
  effect,
  inject,
  input,
  model,
  output,
  viewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { Confirmation } from '../../models/confirmation';
import { ConfirmationService } from '../../services/confirmation.service';
import { SUPPRESS_UNSAVED_CHANGES_WARNING } from '../../tokens/suppress-unsaved-changes-warning.token';
import { ButtonComponent } from '../button/button.component';
import { DismissableModal, ModalDismissMode, ModalRefService } from './modal-ref.service';

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

@Component({
  selector: 'abp-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
  providers: [SubscriptionService],
})
export class ModalComponent implements OnInit, OnDestroy, DismissableModal {
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly modal = inject(NgbModal);
  protected readonly modalRefService = inject(ModalRefService);
  protected readonly suppressUnsavedChangesWarningToken = inject(SUPPRESS_UNSAVED_CHANGES_WARNING, {
    optional: true,
  });
  protected readonly destroyRef = inject(DestroyRef);

  visible = model<boolean>(false);

  busy = input(false, {
    transform: (value: boolean) => {
      if (this.abpSubmit() && this.abpSubmit() instanceof ButtonComponent) {
        this.abpSubmit().loading = value;
      }
      return value;
    },
  });

  options = input<NgbModalOptions>({ keyboard: true });

  suppressUnsavedChangesWarning = input(this.suppressUnsavedChangesWarningToken);

  modalContent = viewChild<TemplateRef<any>>('modalContent');

  abpHeader = contentChild<TemplateRef<any>>('abpHeader');

  abpBody = contentChild<TemplateRef<any>>('abpBody');

  abpFooter = contentChild<TemplateRef<any>>('abpFooter');

  abpSubmit = contentChild(ButtonComponent, { read: ButtonComponent });

  readonly init = output();

  readonly appear = output();

  readonly disappear = output();

  modalRef!: NgbModalRef;

  isConfirmationOpen = false;

  modalIdentifier = `modal-${uuid()}`;

  get modalWindowRef() {
    return document.querySelector(`ngb-modal-window.${this.modalIdentifier}`);
  }

  get isFormDirty(): boolean {
    return Boolean(this.modalWindowRef?.querySelector('.ng-dirty'));
  }

  constructor() {
    effect(() => {
      this.toggle(this.visible());
    });
  }

  ngOnInit(): void {
    this.modalRefService.register(this);
  }

  dismiss(mode: ModalDismissMode) {
    switch (mode) {
      case 'hard':
        this.visible.set(false);
        break;
      case 'soft':
        this.close();
        break;
      default:
        break;
    }
  }

  protected toggle(value: boolean) {
    this.visible.set(value);

    if (!value) {
      this.modalRef?.dismiss();
      this.disappear.emit();
      return;
    }

    setTimeout(() => this.listen(), 0);
    this.modalRef = this.modal.open(this.modalContent(), {
      size: 'md',
      centered: false,
      keyboard: false,
      scrollable: true,
      beforeDismiss: () => {
        if (!this.visible()) return true;

        this.close();
        return !this.visible();
      },
      ...this.options(),
      windowClass: `${this.options().windowClass || ''} ${this.modalIdentifier}`,
    });

    this.appear.emit();
  }

  ngOnDestroy(): void {
    this.modalRefService.unregister(this);
    this.toggle(false);
  }

  close() {
    if (this.busy()) return;

    if (this.isFormDirty && !this.suppressUnsavedChangesWarning()) {
      if (this.isConfirmationOpen) return;

      this.isConfirmationOpen = true;
      this.confirmationService
        .warn('AbpUi::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpUi::AreYouSure', {
          dismissible: false,
        })
        .subscribe((status: Confirmation.Status) => {
          this.isConfirmationOpen = false;
          if (status === Confirmation.Status.confirm) {
            this.visible.set(false);
          }
        });
    } else {
      this.visible.set(false);
    }
  }

  listen() {
    if (this.modalWindowRef) {
      fromEvent<KeyboardEvent>(this.modalWindowRef, 'keyup')
        .pipe(
          takeUntilDestroyed(this.destroyRef),
          debounceTime(150),
          filter((key: KeyboardEvent) => key && key.key === 'Escape' && this.options().keyboard),
        )
        .subscribe(() => this.close());
    }

    fromEvent(window, 'beforeunload')
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(event => {
        if (this.isFormDirty && !this.suppressUnsavedChangesWarning()) {
          event.preventDefault();
        }
      });

    this.init.emit();
  }
}
