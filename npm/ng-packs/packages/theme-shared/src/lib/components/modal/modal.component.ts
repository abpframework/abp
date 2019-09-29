import {
  Component,
  ContentChild,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  Renderer2,
  TemplateRef,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { filter, take, takeUntil, debounceTime } from 'rxjs/operators';
import { Toaster } from '../../models/toaster';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

const ANIMATION_TIMEOUT = 200;

@Component({
  selector: 'abp-modal',
  templateUrl: './modal.component.html',
})
export class ModalComponent implements OnDestroy {
  @Input()
  get visible(): boolean {
    return this._visible;
  }
  set visible(value: boolean) {
    if (typeof value !== 'boolean') return;

    if (!this.modalContent) {
      if (value) {
        setTimeout(() => {
          this.showModal = value;
          this.visible = value;
        }, 0);
      }
      return;
    }

    if (value) {
      this.setVisible(value);
      this.listen();
    } else {
      this.closable = false;
      this.renderer.addClass(this.modalContent.nativeElement, 'fade-out-top');
      setTimeout(() => {
        this.setVisible(value);
        this.ngOnDestroy();
      }, ANIMATION_TIMEOUT - 10);
    }
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

  @Input() centered: boolean = false;

  @Input() modalClass: string = '';

  @Input() size: ModalSize = 'lg';

  @Input() height: number;

  @Input() minHeight: number;

  @Output() visibleChange = new EventEmitter<boolean>();

  @Output() init = new EventEmitter<void>();

  @ContentChild('abpHeader', { static: false }) abpHeader: TemplateRef<any>;

  @ContentChild('abpBody', { static: false }) abpBody: TemplateRef<any>;

  @ContentChild('abpFooter', { static: false }) abpFooter: TemplateRef<any>;

  @ContentChild('abpClose', { static: false, read: ElementRef }) abpClose: ElementRef<any>;

  @ContentChild(ButtonComponent, { static: false, read: ButtonComponent }) abpSubmit: ButtonComponent;

  @ViewChild('abpModalContent', { static: false }) modalContent: ElementRef;

  @ViewChildren('abp-button') abpButtons;

  @Output()
  show = new EventEmitter();

  @Output()
  hide = new EventEmitter();

  _visible: boolean = false;

  _busy: boolean = false;

  showModal: boolean = false;

  isOpenConfirmation: boolean = false;

  closable: boolean = false;

  destroy$ = new Subject<void>();

  constructor(private renderer: Renderer2, private confirmationService: ConfirmationService) {}

  ngOnDestroy(): void {
    this.destroy$.next();
  }

  setVisible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(value);
    this.showModal = value;

    if (value) {
      timer(ANIMATION_TIMEOUT + 100)
        .pipe(take(1))
        .subscribe(_ => (this.closable = true));

      this.renderer.addClass(document.body, 'modal-open');
      this.show.emit();
    } else {
      this.closable = false;
      this.renderer.removeClass(document.body, 'modal-open');
      this.hide.emit();
    }
  }

  listen() {
    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.code === 'Escape' && this.closable),
      )
      .subscribe(_ => {
        this.close();
      });

    setTimeout(() => {
      if (!this.abpClose) return;
      fromEvent(this.abpClose.nativeElement, 'click')
        .pipe(
          takeUntil(this.destroy$),
          filter(() => !!(this.closable && this.modalContent)),
        )
        .subscribe(() => this.close());
    }, 0);

    this.init.emit();
  }

  close() {
    if (!this.closable || this.busy) return;

    const nodes = getFlatNodes(
      (this.modalContent.nativeElement.querySelector('#abp-modal-body') as HTMLElement).childNodes,
    );

    if (hasNgDirty(nodes)) {
      if (this.isOpenConfirmation) return;

      this.isOpenConfirmation = true;
      this.confirmationService
        .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
        .subscribe((status: Toaster.Status) => {
          timer(ANIMATION_TIMEOUT).subscribe(() => {
            this.isOpenConfirmation = false;
          });

          if (status === Toaster.Status.confirm) {
            this.visible = false;
          }
        });
    } else {
      this.visible = false;
    }
  }
}

function getFlatNodes(nodes: NodeList): HTMLElement[] {
  return Array.from(nodes).reduce(
    (acc, val) => [...acc, ...(val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])],
    [],
  );
}

function hasNgDirty(nodes: HTMLElement[]) {
  return nodes.findIndex(node => (node.className || '').indexOf('ng-dirty') > -1) > -1;
}
