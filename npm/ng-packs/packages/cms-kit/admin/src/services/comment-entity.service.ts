import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { ConfigStateService, ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { CommentAdminService, CommentGetListInput } from '@abp/ng.cms-kit/proxy';
import { CMS_KIT_COMMENTS_REQUIRE_APPROVEMENT } from '../components';

@Injectable({
  providedIn: 'root',
})
export class CommentEntityService {
  private commentService = inject(CommentAdminService);
  private toasterService = inject(ToasterService);
  private confirmation = inject(ConfirmationService);
  private configState = inject(ConfigStateService);
  private router = inject(Router);

  get requireApprovement(): boolean {
    return (
      this.configState.getSetting(CMS_KIT_COMMENTS_REQUIRE_APPROVEMENT).toLowerCase() === 'true'
    );
  }

  isCommentReply(commentId: string | undefined): boolean {
    if (!commentId) {
      return false;
    }

    const id = this.router.url.split('/').pop();
    return id === commentId;
  }

  updateApprovalStatus(id: string, isApproved: boolean, list: ListService<CommentGetListInput>) {
    this.commentService
      .updateApprovalStatus(id, { isApproved: isApproved })
      .pipe(tap(() => list.get()))
      .subscribe(() =>
        isApproved
          ? this.toasterService.success('CmsKit::ApprovedSuccessfully')
          : this.toasterService.success('CmsKit::ApprovalRevokedSuccessfully'),
      );
  }

  delete(id: string, list: ListService<CommentGetListInput>) {
    this.confirmation
      .warn('CmsKit::CommentDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        yesText: 'AbpUi::Yes',
        cancelText: 'AbpUi::Cancel',
      })
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.commentService.delete(id).subscribe(() => {
            list.get();
          });
        }
      });
  }
}
