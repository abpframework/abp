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
  Renderer2,
  TemplateRef,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, takeUntil } from 'rxjs/operators';
import { fadeAnimation } from '../../animations/modal.animations';
import { Confirmation } from '../../models/confirmation';
import { ConfirmationService } from '../../services/confirmation.service';
import { ModalService } from '../../services/modal.service';
import { SUPPRESS_UNSAVED_CHANGES_WARNING } from '../../tokens/suppress-unsaved-changes-warning.token';
import { ButtonComponent } from '../button/button.component';

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

@Component({
  selector: 'abp-modal',
  templateUrl: './modal.component.html',
  animations: [fadeAnimation],
  styleUrls: ['./modal.component.scss'],
  providers: [ModalService, SubscriptionService],
})
export class ModalComponent implements OnDestroy {
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

  @Input() centered = false;

  @Input() modalClass = '';

  @Input() size: ModalSize = 'lg';

  @Input() suppressUnsavedChangesWarning = this.suppressUnsavedChangesWarningToken;

  @ContentChild(ButtonComponent, { static: false, read: ButtonComponent })
  abpSubmit: ButtonComponent;

  @ContentChild('abpHeader', { static: false }) abpHeader: TemplateRef<any>;

  @ContentChild('abpBody', { static: false }) abpBody: TemplateRef<any>;

  @ContentChild('abpFooter', { static: false }) abpFooter: TemplateRef<any>;

  @ContentChild('abpClose', { static: false, read: ElementRef })
  abpClose: ElementRef<any>;

  @ViewChild('template', { static: false }) template: TemplateRef<any>;

  @ViewChild('abpModalContent', { static: false }) modalContent: ElementRef;

  @ViewChildren('abp-button') abpButtons;

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Output() readonly init = new EventEmitter<void>();

  @Output() readonly appear = new EventEmitter();

  @Output() readonly disappear = new EventEmitter();

  _visible = false;

  _busy = false;

  isModalOpen = false;

  isConfirmationOpen = false;

  destroy$ = new Subject<void>();

  private toggle$ = new Subject<boolean>();

  get isFormDirty(): boolean {
    return Boolean(document.querySelector('.modal-dialog .ng-dirty'));
  }

  constructor(
    private renderer: Renderer2,
    private confirmationService: ConfirmationService,
    private modalService: ModalService,
    private subscription: SubscriptionService,
    @Optional()
    @Inject(SUPPRESS_UNSAVED_CHANGES_WARNING)
    private suppressUnsavedChangesWarningToken: boolean,
  ) {
    this.initToggleStream();
  }

  private initToggleStream() {
    this.subscription.addOne(this.toggle$.pipe(debounceTime(0), distinctUntilChanged()), value =>
      this.toggle(value),
    );
  }

  private toggle(value: boolean) {
    this.isModalOpen = value;
    this._visible = value;
    this.visibleChange.emit(value);

    if (value) {
      this.modalService.renderTemplate(this.template);
      setTimeout(() => this.listen(), 0);
      this.renderer.addClass(document.body, 'modal-open');
      this.appear.emit();
    } else {
      this.modalService.clearModal();
      this.renderer.removeClass(document.body, 'modal-open');
      this.disappear.emit();
      this.destroy$.next();
    }
  }

  ngOnDestroy(): void {
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
        if (this.isFormDirty) {
          event.returnValue = true;
        } else {
          event.returnValue = false;
          delete event.returnValue;
        }
      });

    setTimeout(() => {
      if (!this.abpClose) return;
      fromEvent(this.abpClose.nativeElement, 'click')
        .pipe(
          takeUntil(this.destroy$),
          filter(() => !!this.modalContent),
        )
        .subscribe(() => this.close());
    }, 0);

    this.init.emit();
  }
}
