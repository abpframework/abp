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
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter, takeUntil } from 'rxjs/operators';
import { fadeAnimation } from '../../animations/modal.animations';
import { Toaster } from '../../models/toaster';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';

export type ModalSize = 'sm' | 'md' | 'lg' | 'xl';

@Component({
  selector: 'abp-modal',
  templateUrl: './modal.component.html',
  animations: [fadeAnimation],
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnDestroy {
  @Input()
  get visible(): boolean {
    return this._visible;
  }
  set visible(value: boolean) {
    if (typeof value !== 'boolean') return;

    this.isModalOpen = value;
    this._visible = value;
    this.visibleChange.emit(value);

    if (value) {
      setTimeout(() => this.listen(), 0);
      this.renderer.addClass(document.body, 'modal-open');
      this.appear.emit();
    } else {
      this.renderer.removeClass(document.body, 'modal-open');
      this.disappear.emit();
      this.destroy$.next();
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

  @Input() centered = false;

  @Input() modalClass = '';

  @Input() size: ModalSize = 'lg';

  @ContentChild(ButtonComponent, { static: false, read: ButtonComponent })
  abpSubmit: ButtonComponent;

  @ContentChild('abpHeader', { static: false }) abpHeader: TemplateRef<any>;

  @ContentChild('abpBody', { static: false }) abpBody: TemplateRef<any>;

  @ContentChild('abpFooter', { static: false }) abpFooter: TemplateRef<any>;

  @ContentChild('abpClose', { static: false, read: ElementRef })
  abpClose: ElementRef<any>;

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

  get isFormDirty(): boolean {
    let node: HTMLDivElement;
    if (!this.modalContent) {
      node = document.getElementById('modal-container') as HTMLDivElement;
    }

    const nodes = getFlatNodes(
      ((node || this.modalContent.nativeElement).querySelector('#abp-modal-body') as HTMLElement)
        .childNodes,
    );

    return hasNgDirty(nodes);
  }

  constructor(private renderer: Renderer2, private confirmationService: ConfirmationService) {}

  ngOnDestroy(): void {
    this.destroy$.next();
  }

  close() {
    if (this.busy) return;

    if (this.isFormDirty) {
      if (this.isConfirmationOpen) return;

      this.isConfirmationOpen = true;
      this.confirmationService
        .warn(
          'AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage',
          'AbpAccount::AreYouSure',
        )
        .subscribe((status: Toaster.Status) => {
          this.isConfirmationOpen = false;
          if (status === Toaster.Status.confirm) {
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
      .subscribe(() => {
        this.close();
      });

    fromEvent(window, 'beforeunload')
      .pipe(takeUntil(this.destroy$))
      .subscribe(event => {
        if (this.isFormDirty) {
          event.returnValue = true;
        } else {
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

function getFlatNodes(nodes: NodeList): HTMLElement[] {
  return Array.from(nodes).reduce(
    (acc, val) => [
      ...acc,
      ...(val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val]),
    ],
    [],
  );
}

function hasNgDirty(nodes: HTMLElement[]) {
  return nodes.findIndex(node => (node.className || '').indexOf('ng-dirty') > -1) > -1;
}
