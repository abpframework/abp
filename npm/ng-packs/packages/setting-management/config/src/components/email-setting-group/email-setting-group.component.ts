import { collapse, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { EmailSettingsService } from '../../proxy/email-settings.service';
import { EmailSettingsDto } from '../../proxy/models';

@Component({
  selector: 'abp-email-setting-group',
  templateUrl: 'email-setting-group.component.html',
  animations: [collapse],
})
export class EmailSettingGroupComponent implements OnInit {
  form!: FormGroup;

  saving = false;

  constructor(
    private emailSettingsService: EmailSettingsService,
    private fb: FormBuilder,
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
        this.toasterService.success('AbpSettingManagement::SuccessfullySaved');
        this.getData();
      });
  }
}
