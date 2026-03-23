import { Component, OnInit, input, signal, effect } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { toastInOut } from '../../animations/toast.animations';
import { Toaster } from '../../models/toaster';
import { ToastComponent } from '../toast/toast.component';

@Component({
  selector: 'abp-toast-container',
  templateUrl: './toast-container.component.html',
  styleUrls: ['./toast-container.component.scss'],
  animations: [toastInOut],
  imports: [ToastComponent],
  host: {
    '(window:resize)': 'onWindowResize()'
  }
})
export class ToastContainerComponent implements OnInit {
  toasts$!: ReplaySubject<Toaster.Toast[]>;

  remove!: (toastId: number) => void;

  toasts = [] as Toaster.Toast[];

  readonly top = input<string | undefined>(undefined);
  readonly rightInput = input('30px', { alias: 'right' });
  readonly bottom = input('30px');
  readonly left = input<string | undefined>(undefined);
  readonly toastKey = input<string | undefined>(undefined);

  protected readonly right = signal('30px');
  readonly defaultRight = '30px';
  readonly defaultMobileRight = '0';

  constructor() {
    effect(() => {
      this.right.set(this.rightInput());
    });
  }

  ngOnInit() {
    this.setDefaultRight();
    this.toasts$.subscribe(toasts => {
      this.toasts = this.toastKey()
        ? toasts.filter(t => {
            return t.options && t.options.containerKey !== this.toastKey();
          })
        : toasts;
    });
  }

  onWindowResize() {
    this.setDefaultRight();
  }

  setDefaultRight() {
    const screenWidth = window.innerWidth;
    if (screenWidth < 768 && this.right() === this.defaultRight) {
      this.right.set(this.defaultMobileRight);
    }
  }

  trackByFunc(index: number, toast: Toaster.Toast) {
    if (!toast) return null;
    return toast.options?.id;
  }
}
