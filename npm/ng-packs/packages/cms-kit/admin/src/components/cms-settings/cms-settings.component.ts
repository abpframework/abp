import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  NgbNav,
  NgbNavItem,
  NgbNavItemRole,
  NgbNavLink,
  NgbNavLinkBase,
  NgbNavContent,
  NgbNavOutlet,
} from '@ng-bootstrap/ng-bootstrap';
import { finalize } from 'rxjs/operators';
import { ConfigStateService, LocalizationPipe } from '@abp/ng.core';
import { ButtonComponent, ToasterService } from '@abp/ng.theme.shared';
import { CommentAdminService } from '@abp/ng.cms-kit/proxy';

import { CMS_KIT_COMMENTS_REQUIRE_APPROVEMENT } from '../comments';

@Component({
  selector: 'abp-cms-settings',
  templateUrl: './cms-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    NgbNav,
    NgbNavItem,
    NgbNavItemRole,
    NgbNavLink,
    NgbNavLinkBase,
    NgbNavContent,
    NgbNavOutlet,
    LocalizationPipe,
    ButtonComponent,
    ReactiveFormsModule,
  ],
})
export class CmsSettingsComponent implements OnInit {
  readonly commentAdminService = inject(CommentAdminService);
  readonly configState = inject(ConfigStateService);
  readonly toaster = inject(ToasterService);

  readonly loading = signal(false);

  commentApprovalControl = new FormControl(false);

  ngOnInit() {
    const isCommentApprovalEnabled =
      this.configState.getSetting(CMS_KIT_COMMENTS_REQUIRE_APPROVEMENT).toLowerCase() === 'true';
    this.commentApprovalControl.setValue(isCommentApprovalEnabled);
  }

  submit() {
    this.loading.set(true);
    this.commentAdminService
      .updateSettings({ commentRequireApprovement: this.commentApprovalControl.value })
      .pipe(
        finalize(() => {
          this.loading.set(false);
          this.configState.refreshAppState().subscribe();
        }),
      )
      .subscribe(() => this.toaster.success('AbpUi::SavedSuccessfully', null));
  }
}
