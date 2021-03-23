import { SubscriptionService } from '@abp/ng.core';
import {
  Component,
  ContentChild,
  ElementRef,
  EventEmitter,
  Inject,
  Input,
  OnDestroy,
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

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

@Component({
  selector: 'abp-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
  providers: [SubscriptionService],
})
export class ModalComponent implements OnDestroy {
  /**
   * @deprecated Use centered property of options input instead. To be deleted in v5.0.
   */
  @Input() centered = false;
  /**
   * @deprecated Use windowClass property of options input instead. To be deleted in v5.0.
   */
  @Input() modalClass = '';
  /**
   * @deprecated Use size property of options input instead. To be deleted in v5.0.
   */
  @Input() size: ModalSize = 'lg';

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

  @ContentChild('abpClose', { static: false, read: ElementRef })
  abpClose: ElementRef<any>;

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Output() readonly init = new EventEmitter<void>();

  @Output() readonly appear = new EventEmitter<void>();

  @Output() readonly disappear = new EventEmitter<void>();

  _visible = false;

  _busy = false;

  modalRef: NgbModalRef;

  isConfirmationOpen = false;

  destroy$ = new Subject<void>();

  private toggle$ = new Subject<boolean>();

  get isFormDirty(): boolean {
    return Boolean(document.querySelector('.modal-dialog .ng-dirty'));
  }

  constructor(
    private confirmationService: ConfirmationService,
    private subscription: SubscriptionService,
    @Optional()
    @Inject(SUPPRESS_UNSAVED_CHANGES_WARNING)
    private suppressUnsavedChangesWarningToken: boolean,
    private modal: NgbModal,
  ) {
    this.initToggleStream();
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
      // TODO: set size to 'lg' when removed the size variable
      size: this.size,
      windowClass: this.modalClass,
      centered: this.centered,
      keyboard: false,
      scrollable: true,
      beforeDismiss: () => {
        if (!this.visible) return true;

        this.close();
        return !this.visible;
      },
      ...this.options,
    });

    this.appear.emit();
  }

  ngOnDestroy(): void {
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
    fromEvent(document, 'keyup')
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

    setTimeout(() => {
      if (!this.abpClose) return;
      fromEvent(this.abpClose.nativeElement, 'click')
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => this.close());
    }, 0);

    this.init.emit();
  }
}
