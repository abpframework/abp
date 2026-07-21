import {Component, input, OnInit, signal, effect, ChangeDetectionStrategy,} from '@angular/core';
import { Toaster } from '../../models/toaster';
import { ToastComponent } from '../toast/toast.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-toast-container',
  templateUrl: './toast-container.component.html',
  styleUrls: ['./toast-container.component.scss'],
  imports: [ToastComponent],
  host: {
    class: 'abp-toast-host',
    '(window:resize)': 'onWindowResize()',
  },
})
export class ToastContainerComponent implements OnInit {
  remove!: (toastId: number) => void;

  readonly toasts = signal<Toaster.Toast[]>([]);

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
  }

  setToasts(toasts: Toaster.Toast[]) {
    const key = this.toastKey();
    this.toasts.set(
      key ? toasts.filter(t => t.options && t.options.containerKey !== key) : [...toasts],
    );
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
}
