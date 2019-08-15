import { ChangePassword } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { comparePasswords } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import snq from 'snq';
import { finalize } from 'rxjs/operators';

const { minLength, required } = Validators;

@Component({
  selector: 'abp-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent implements OnInit, OnChanges {
  protected _visible;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(value);
  }

  @Output()
  visibleChange = new EventEmitter<boolean>();

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  form: FormGroup;

  modalBusy: boolean = false;

  constructor(private fb: FormBuilder, private store: Store, private toasterService: ToasterService) {}

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        password: ['', required],
        newPassword: ['', required],
        repeatNewPassword: ['', required],
      },
      {
        validators: [comparePasswords(['newPassword', 'repeatNewPassword'])],
      },
    );
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.modalBusy = true;

    this.store
      .dispatch(
        new ChangePassword({
          currentPassword: this.form.get('password').value,
          newPassword: this.form.get('newPassword').value,
        }),
      )
      .pipe(
        finalize(() => {
          this.modalBusy = false;
        }),
      )
      .subscribe({
        next: () => {
          this.visible = false;
          this.form.reset();
        },
        error: err => {
          this.toasterService.error(snq(() => err.error.error.message, 'AbpAccount::DefaultErrorMessage'), 'Error', {
            life: 7000,
          });
        },
      });
  }

  openModal() {
    this.visible = true;
  }

  ngOnChanges({ visible }: SimpleChanges): void {
    if (!visible) return;

    if (visible.currentValue) {
      this.openModal();
    } else if (visible.currentValue === false && this.visible) {
      this.visible = false;
    }
  }
}
