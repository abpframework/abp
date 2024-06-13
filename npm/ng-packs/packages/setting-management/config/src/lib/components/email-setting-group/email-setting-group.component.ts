import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { collapse, ToasterService } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { SettingManagementPolicyNames } from '../../enums/policy-names';
import { EmailSettingsService } from '@abp/ng.setting-management/proxy';
import { EmailSettingsDto } from '../../proxy/models';
import { ConfigStateService, LocalizationService } from '@abp/ng.core';

const { required, email } = Validators;

@Component({
  selector: 'abp-email-setting-group',
  templateUrl: 'email-setting-group.component.html',
  animations: [collapse],
})
export class EmailSettingGroupComponent implements OnInit {
  protected readonly localizationService = inject(LocalizationService);
  protected readonly configStateSevice = inject(ConfigStateService);
  protected readonly currentUserEmail = toSignal(
    this.configStateSevice.getDeep$(['currentUser', 'email']),
  );

  form!: UntypedFormGroup;
  emailTestForm: UntypedFormGroup;
  saving = false;
  emailingPolicy = SettingManagementPolicyNames.Emailing;
  isEmailTestModalOpen = false;
  modalSize: NgbModalOptions = { size: 'lg' };

  constructor(
    private emailSettingsService: EmailSettingsService,
    private fb: UntypedFormBuilder,
    private toasterService: ToasterService,
  ) {}

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.emailSettingsService.get().subscribe(res => {
      this.buildForm(res);
    });
  }

  private buildForm(emailSettings: EmailSettingsDto) {
    this.form = this.fb.group({
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
    if (this.saving || this.form.invalid) return;

    this.saving = true;
    this.emailSettingsService
      .update(this.form.value)
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
    const { defaultFromAddress } = this.form.value || {};
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

    this.emailSettingsService.sendTestEmail(this.emailTestForm.value).subscribe(res => {
      this.toasterService.success('AbpSettingManagement::SuccessfullySent');
      this.isEmailTestModalOpen = false;
    });
  }
}
