import { ProfileChangePassword } from '@abp/ng.core';
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
import { comparePasswords, validatePassword } from '@ngx-validate/core';
import { Store } from '@ngxs/store';

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

  constructor(private fb: FormBuilder, private store: Store) {}

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        password: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
        newPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
        repeatNewPassword: ['', [required, minLength(6), validatePassword(['small', 'capital', 'number', 'special'])]],
      },
      {
        validators: [comparePasswords(['newPassword', 'repeatNewPassword'])],
      },
    );
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.store
      .dispatch(
        new ProfileChangePassword({
          currentPassword: this.form.get('password').value,
          newPassword: this.form.get('newPassword').value,
        }),
      )
      .subscribe(() => {
        this.visible = false;
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
