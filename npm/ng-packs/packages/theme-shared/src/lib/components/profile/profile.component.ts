import { Profile, GetProfile, ProfileState, UpdateProfile } from '@abp/ng.core';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';

const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent implements OnChanges {
  protected _visible;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(value);
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Select(ProfileState.getProfile)
  profile$: Observable<Profile.Response>;

  form: FormGroup;

  modalBusy = false;

  constructor(private fb: FormBuilder, private store: Store) {}

  buildForm() {
    this.store
      .dispatch(new GetProfile())
      .pipe(
        withLatestFrom(this.profile$),
        take(1)
      )
      .subscribe(([, profile]) => {
        this.form = this.fb.group({
          userName: [profile.userName, [required, maxLength(256)]],
          email: [profile.email, [required, email, maxLength(256)]],
          name: [profile.name || '', [maxLength(64)]],
          surname: [profile.surname || '', [maxLength(64)]],
          phoneNumber: [profile.phoneNumber || '', [maxLength(16)]]
        });
      });
  }

  submit() {
    if (this.form.invalid) return;
    this.modalBusy = true;

    this.store.dispatch(new UpdateProfile(this.form.value)).subscribe(() => {
      this.modalBusy = false;
      this.visible = false;
      this.form.reset();
    });
  }

  openModal() {
    this.buildForm();
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
