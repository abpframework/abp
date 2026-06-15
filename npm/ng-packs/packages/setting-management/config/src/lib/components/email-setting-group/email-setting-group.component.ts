import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import {
  ButtonComponent,
  ModalCloseDirective,
  ModalComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import { Component, inject, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { SettingManagementPolicyNames } from '../../enums/policy-names';
import { EmailSettingsService } from '@abp/ng.setting-management/proxy';
import { EmailSettingsDto } from '../../proxy/models';
import {
  ConfigStateService,
  LocalizationPipe,
  LocalizationService,
  PermissionDirective,
} from '@abp/ng.core';
import { NgxValidateCoreModule } from '@ngx-validate/core';

const { required, email } = Validators;

@Component({
  selector: 'abp-email-setting-group',
  templateUrl: 'email-setting-group.component.html',
  imports: [
    ReactiveFormsModule,
    LocalizationPipe,
    ButtonComponent,
    ModalComponent,
    ModalCloseDirective,
    NgxValidateCoreModule,
    PermissionDirective,
  ],
})
export class EmailSettingGroupComponent implements OnInit {
  private emailSettingsService = inject(EmailSettingsService);
  private fb = inject(UntypedFormBuilder);
  private toasterService = inject(ToasterService);

  protected readonly localizationService = inject(LocalizationService);
  protected readonly configStateSevice = inject(ConfigStateService);
  protected readonly currentUserEmail = toSignal(
    this.configStateSevice.getDeep$(['currentUser', 'email']),
  );

  readonly form = signal<UntypedFormGroup | undefined>(undefined);
  readonly loading = signal(true);

  emailTestForm!: UntypedFormGroup;
  saving = false;
  emailingPolicy = SettingManagementPolicyNames.Emailing;
  isEmailTestModalOpen = false;
  modalSize: NgbModalOptions = { size: 'lg' };

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.loading.set(true);
    this.emailSettingsService.get().subscribe({
      next: res => {
        this.form.set(this.buildForm(res));
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  private buildForm(emailSettings: EmailSettingsDto) {
    return this.fb.group({
      defaultFromDisplayName: [emailSettings.defaultFromDisplayName, [Validators.required]],
      defaultFromAddress: [emailSettings.defaultFromAddress, [Validators.required]],
      smtpHost: [emailSettings.smtpHost],
      smtpPort: [emailSettings.smtpPort, [Validators.required]],
      smtpEnableSsl: [emailSettings.smtpEnableSsl],
      smtpUseDefaultCredentials: [emailSettings.smtpUseDefaultCredentials],
      smtpDomain: [emailSettings.smtpDomain],
      smtpUserName: [emailSettings.smtpUserName],
      smtpPassword: [emailSettings.smtpPassword],
    });
  }

  submit() {
    const form = this.form();
    if (!form || this.saving || form.invalid) return;

    this.saving = true;
    this.emailSettingsService
      .update(form.value)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.toasterService.success('AbpSettingManagement::SavedSuccessfully');
        this.getData();
      });
  }

  openSendEmailModal() {
    this.buildEmailTestForm();
    this.isEmailTestModalOpen = true;
  }

  buildEmailTestForm() {
    const { defaultFromAddress } = this.form()?.value || {};
    const defaultSubject = this.localizationService.instant(
      'AbpSettingManagement::TestEmailSubject',
      ...[Math.floor(Math.random() * 9999).toString()],
    );
    const defaultBody = this.localizationService.instant('AbpSettingManagement::TestEmailBody');

    this.emailTestForm = this.fb.group({
      senderEmailAddress: [defaultFromAddress || '', [required, email]],
      targetEmailAddress: [this.currentUserEmail(), [required, email]],
      subject: [defaultSubject, [required]],
      body: [defaultBody],
    });
  }

  emailTestFormSubmit() {
    if (this.emailTestForm.invalid) {
      return;
    }

    this.emailSettingsService.sendTestEmail(this.emailTestForm.value).subscribe(() => {
      this.toasterService.success('AbpSettingManagement::SentSuccessfully');
      this.isEmailTestModalOpen = false;
    });
  }
}
