import { AuthWrapperService } from '@abp/ng.account.core';
import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { LocalizationPipe, ReplaceableTemplateDirective } from '@abp/ng.core';
import { TenantBoxComponent } from '../tenant-box/tenant-box.component';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  providers: [AuthWrapperService],
  imports: [AsyncPipe, TenantBoxComponent, ReplaceableTemplateDirective, LocalizationPipe],
})
export class AuthWrapperComponent {
  service = inject(AuthWrapperService);
}