import { Profile, ProfileGet, ProfileState, ProfileUpdate } from '@abp/ng.core';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { take, withLatestFrom } from 'rxjs/operators';

const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-profile',
  templateUrl: './profile.component.html',
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

  @Output()
  visibleChange = new EventEmitter<boolean>();

  @Select(ProfileState.getProfile)
  profile$: Observable<Profile.Response>;

  form: FormGroup;

  constructor(private fb: FormBuilder, private store: Store) {}

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

    this.store.dispatch(new ProfileUpdate(this.form.value)).subscribe(() => {
      this.visible = false;
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
