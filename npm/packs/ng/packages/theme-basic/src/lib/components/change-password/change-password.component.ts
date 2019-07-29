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
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { comparePasswords, validatePassword } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import { from } from 'rxjs';
import { take } from 'rxjs/operators';

const { minLength, required } = Validators;

@Component({
  selector: 'abp-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent implements OnInit, OnChanges {
  @Input()
  visible: boolean;

  @Output()
  visibleChange = new EventEmitter<boolean>();

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  form: FormGroup;

  modalRef: NgbModalRef;

  constructor(private fb: FormBuilder, private modalService: NgbModal, private store: Store) {}

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
      .subscribe(() => this.modalRef.close());
  }

  openModal() {
    this.modalRef = this.modalService.open(this.modalContent);
    this.visibleChange.emit(true);

    from(this.modalRef.result)
      .pipe(take(1))
      .subscribe(
        data => {
          this.setVisible(false);
        },
        reason => {
          this.setVisible(false);
        },
      );
  }

  setVisible(value: boolean) {
    this.visible = value;
    this.visibleChange.emit(value);
  }

  ngOnChanges({ visible }: SimpleChanges): void {
    if (!visible) return;

    if (visible.currentValue) {
      this.openModal();
    } else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
      this.modalRef.close();
    }
  }
}
