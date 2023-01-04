import { Component, HostListener, Input, OnInit } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { toastInOut } from '../../animations/toast.animations';
import { Toaster } from '../../models/toaster';

@Component({
  selector: 'abp-toast-container',
  templateUrl: './toast-container.component.html',
  styleUrls: ['./toast-container.component.scss'],
  animations: [toastInOut],
})
export class ToastContainerComponent implements OnInit {
  toasts$!: ReplaySubject<Toaster.Toast[]>;

  remove!: (toastId: number) => void;

  toasts = [] as Toaster.Toast[];

  @Input()
  top?: string;

  @Input()
  right = '30px';
  defaultRight = '30px';
  defaultMobileRight = '0';

  @Input()
  bottom = '30px';

  @Input()
  left?: string;

  @Input()
  toastKey?: string;

  ngOnInit() {
    this.setDefaultRight();
    this.toasts$.subscribe(toasts => {
      this.toasts = this.toastKey
        ? toasts.filter(t => {
            return t.options && t.options.containerKey !== this.toastKey;
          })
        : toasts;
    });
  }

  @HostListener('window:resize', ['$event'])
  onWindowResize() {
    this.setDefaultRight();
  }

  setDefaultRight() {
    const screenWidth = window.innerWidth;
    if (screenWidth < 768 && this.right == this.defaultRight) {
      this.right = this.defaultMobileRight;
    }
  }

  trackByFunc(index: number, toast: Toaster.Toast) {
    if (!toast) return null;
    return toast.options?.id;
  }
}
