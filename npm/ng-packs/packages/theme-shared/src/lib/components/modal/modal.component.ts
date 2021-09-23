import { SubscriptionService, uuid } from '@abp/ng.core';
import {
  Component,
  ContentChild,
  EventEmitter,
  Inject,
  Input,
  OnDestroy,
  OnInit,
  Optional,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, takeUntil } from 'rxjs/operators';
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
  @Input()
  get visible(): boolean {
    return this._visible;
  }
  set visible(value: boolean) {
    if (typeof value !== 'boolean') return;
    this.toggle$.next(value);
  }

  @Input()
  get busy(): boolean {
    return this._busy;
  }
  set busy(value: boolean) {
    if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
      this.abpSubmit.loading = value;
    }

    this._busy = value;
  }

  @Input() options: NgbModalOptions = {};

  @Input() suppressUnsavedChangesWarning = this.suppressUnsavedChangesWarningToken;

  @ViewChild('modalContent') modalContent: TemplateRef<any>;

  @ContentChild('abpHeader', { static: false }) abpHeader: TemplateRef<any>;

  @ContentChild('abpBody', { static: false }) abpBody: TemplateRef<any>;

  @ContentChild('abpFooter', { static: false }) abpFooter: TemplateRef<any>;

  @ContentChild(ButtonComponent, { static: false, read: ButtonComponent })
  abpSubmit: ButtonComponent;

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Output() readonly init = new EventEmitter<void>();

  @Output() readonly appear = new EventEmitter<void>();

  @Output() readonly disappear = new EventEmitter<void>();

  _visible = false;

  _busy = false;

  modalRef: NgbModalRef;

  isConfirmationOpen = false;

  destroy$ = new Subject<void>();

  modalIdentifier = `modal-${uuid()}`;

  private toggle$ = new Subject<boolean>();

  get modalWindowRef() {
    return document.querySelector(`ngb-modal-window.${this.modalIdentifier}`);
  }

  get isFormDirty(): boolean {
    return Boolean(this.modalWindowRef.querySelector('.ng-dirty'));
  }

  constructor(
    private confirmationService: ConfirmationService,
    private subscription: SubscriptionService,
    @Optional()
    @Inject(SUPPRESS_UNSAVED_CHANGES_WARNING)
    private suppressUnsavedChangesWarningToken: boolean,
    private modal: NgbModal,
    private modalRefService: ModalRefService,
  ) {
    this.initToggleStream();
  }
  ngOnInit(): void {
    this.modalRefService.register(this);
  }

  dismiss(mode: ModalDismissMode) {
    switch (mode) {
      case 'hard':
        this.visible = false;
        break;
      case 'soft':
        this.close();
        break;
      default:
        break;
    }
  }

  private initToggleStream() {
    this.subscription.addOne(this.toggle$.pipe(debounceTime(0), distinctUntilChanged()), value =>
      this.toggle(value),
    );
  }

  private toggle(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(value);

    if (!value) {
      this.modalRef?.dismiss();
      this.disappear.emit();
      this.destroy$.next();
      return;
    }

    setTimeout(() => this.listen(), 0);
    this.modalRef = this.modal.open(this.modalContent, {
      size: 'lg',
      centered: true,
      keyboard: false,
      scrollable: true,
      beforeDismiss: () => {
        if (!this.visible) return true;

        this.close();
        return !this.visible;
      },
      ...this.options,
      windowClass: `${this.options.windowClass || ''} ${this.modalIdentifier}`,
    });

    this.appear.emit();
  }

  ngOnDestroy(): void {
    this.modalRefService.unregister(this);
    this.toggle(false);
    this.destroy$.next();
  }

  close() {
    if (this.busy) return;

    if (this.isFormDirty && !this.suppressUnsavedChangesWarning) {
      if (this.isConfirmationOpen) return;

      this.isConfirmationOpen = true;
      this.confirmationService
        .warn(
          'AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage',
          'AbpAccount::AreYouSure',
          { dismissible: false },
        )
        .subscribe((status: Confirmation.Status) => {
          this.isConfirmationOpen = false;
          if (status === Confirmation.Status.confirm) {
            this.visible = false;
          }
        });
    } else {
      this.visible = false;
    }
  }

  listen() {
    fromEvent(this.modalWindowRef, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.key === 'Escape'),
      )
      .subscribe(() => this.close());

    fromEvent(window, 'beforeunload')
      .pipe(takeUntil(this.destroy$))
      .subscribe(event => {
        event.preventDefault();
        if (this.isFormDirty && !this.suppressUnsavedChangesWarning) {
          event.returnValue = true;
        } else {
          delete event.returnValue;
        }
      });

    this.init.emit();
  }
}
