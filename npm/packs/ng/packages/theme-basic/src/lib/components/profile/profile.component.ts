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
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Store, Select } from '@ngxs/store';
import { from, Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';
import { ProfileGet, ProfileState, Profile, ProfileUpdate } from '@abp/ng.core';

const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnChanges {
  @Input()
  visible: boolean;

  @Output()
  visibleChange = new EventEmitter<boolean>();

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  @Select(ProfileState.getProfile)
  profile$: Observable<Profile.Response>;

  form: FormGroup;

  modalRef: NgbModalRef;

  constructor(private fb: FormBuilder, private modalService: NgbModal, private store: Store) {}

  buildForm() {
    this.store
      .dispatch(new ProfileGet())
      .pipe(
        withLatestFrom(this.profile$),
        take(1),
      )
      .subscribe(([, profile]) => {
        this.form = this.fb.group({
          userName: [profile.userName, [required, maxLength(256)]],
          email: [profile.email, [required, email, maxLength(256)]],
          name: [profile.name || '', [maxLength(64)]],
          surname: [profile.surname || '', [maxLength(64)]],
          phoneNumber: [profile.phoneNumber || '', [maxLength(16)]],
        });
      });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.store.dispatch(new ProfileUpdate(this.form.value)).subscribe(() => this.modalRef.close());
  }

  openModal() {
    this.buildForm();

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
