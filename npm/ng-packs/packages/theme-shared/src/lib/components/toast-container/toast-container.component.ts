import { Component, Input, OnInit } from '@angular/core';
import { Toaster } from '../../models/toaster';
import { toastInOut } from '../../animations/toast.animations';
import { ToasterService } from '../../services/toaster.service';

@Component({
  selector: 'abp-toast-container',
  templateUrl: './toast-container.component.html',
  styleUrls: ['./toast-container.component.scss'],
  animations: [toastInOut],
})
export class ToastContainerComponent implements OnInit {
  toasts = [] as Toaster.Toast[];

  @Input()
  top: string;

  @Input()
  right = '30px';

  @Input()
  bottom = '30px';

  @Input()
  left: string;

  @Input()
  toastKey: string;

  constructor(private toastService: ToasterService) {}

  ngOnInit() {
    this.toastService.toasts$.subscribe(toasts => {
      this.toasts = this.toastKey
        ? toasts.filter(t => {
            return t.options && t.options.containerKey !== this.toastKey;
          })
        : toasts;
    });
  }

  trackByFunc(index, toast) {
    if (!toast) return null;
    return toast.options.id;
  }
}
