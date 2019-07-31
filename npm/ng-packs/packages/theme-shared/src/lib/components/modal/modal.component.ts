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
} from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { debounceTime, filter, take, takeUntil } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
import { Toaster } from '../../models/toaster';

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

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
    if (!this.modalContent) {
      setTimeout(() => (this.visible = value), 0);
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
        this.renderer.removeClass(this.modalContent.nativeElement, 'fade-out-top');
        this.ngOnDestroy();
      }, 350);
    }
  }

  @Input() centered: boolean = true;

  @Input() modalClass: string = '';

  @Input() size: ModalSize = 'lg';

  @Output() visibleChange = new EventEmitter<boolean>();

  @ContentChild('abpHeader', { static: false }) abpHeader: TemplateRef<any>;

  @ContentChild('abpBody', { static: false }) abpBody: TemplateRef<any>;

  @ContentChild('abpFooter', { static: false }) abpFooter: TemplateRef<any>;

  @ContentChild('abpClose', { static: false, read: ElementRef }) abpClose: ElementRef<any>;

  @ViewChild('abpModalContent', { static: false }) modalContent: ElementRef;

  _visible: boolean = false;

  closable: boolean = false;

  isOpenConfirmation: boolean = false;

  destroy$ = new Subject<void>();

  constructor(private renderer: Renderer2, private confirmationService: ConfirmationService) {}

  ngOnDestroy(): void {
    this.destroy$.next();
  }

  setVisible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(value);
    value
      ? timer(500)
          .pipe(take(1))
          .subscribe(_ => (this.closable = true))
      : (this.closable = false);
  }

  listen() {
    fromEvent(document, 'click')
      .pipe(
        debounceTime(350),
        takeUntil(this.destroy$),
        filter(
          (event: MouseEvent) =>
            event &&
            this.closable &&
            this.modalContent &&
            !this.isOpenConfirmation &&
            !this.modalContent.nativeElement.contains(event.target),
        ),
      )
      .subscribe(_ => {
        this.close();
      });

    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        filter((key: KeyboardEvent) => key && key.code === 'Escape' && this.closable),
        debounceTime(350),
      )
      .subscribe(_ => {
        this.close();
      });

    if (!this.abpClose) return;

    fromEvent(this.abpClose.nativeElement, 'click')
      .pipe(
        takeUntil(this.destroy$),
        filter(() => !!(this.closable && this.modalContent)),
        debounceTime(350),
      )
      .subscribe(() => this.close());
  }

  close() {
    const nodes = getFlatNodes(
      (this.modalContent.nativeElement.querySelector('#abp-modal-body') as HTMLElement).childNodes,
    );

    if (hasNgDirty(nodes)) {
      if (this.isOpenConfirmation) return;

      this.isOpenConfirmation = true;
      this.confirmationService
        .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
        .subscribe((status: Toaster.Status) => {
          timer(400).subscribe(() => {
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
    (acc, val) => [...acc, ...(val.childNodes && val.childNodes.length ? Array.from(val.childNodes) : [val])],
    [],
  );
}

function hasNgDirty(nodes: HTMLElement[]) {
  return nodes.findIndex(node => (node.className || '').indexOf('ng-dirty') > -1) > -1;
}
