import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateAdapter, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { ListService, LocalizationPipe, PagedResultDto } from '@abp/ng.core';
import { PageComponent } from '@abp/ng.components/page';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { ButtonComponent, DateTimeAdapter, FormInputComponent } from '@abp/ng.theme.shared';
import {
  CommentAdminService,
  CommentGetListInput,
  CommentWithAuthorDto,
  CommentApproveState,
  commentApproveStateOptions,
} from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import { CommentEntityService } from '../../../services';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-comment-details',
  templateUrl: './comment-details.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.CommentDetails,
    },
  ],
  viewProviders: [
    {
      provide: NgbDateAdapter,
      useClass: DateTimeAdapter,
    },
  ],
  imports: [
    ExtensibleTableComponent,
    PageComponent,
    LocalizationPipe,
    ReactiveFormsModule,
    NgbDatepickerModule,
    CommonModule,
    DatePipe,
    FormInputComponent,
    ButtonComponent,
  ],
})
export class CommentDetailsComponent implements OnInit {
  readonly comment = signal<CommentWithAuthorDto | null>(null);
  readonly commentId = signal<string | null>(null);

  readonly list = inject(ListService<CommentGetListInput>);
  readonly commentEntityService = inject(CommentEntityService);

  private commentService = inject(CommentAdminService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  readonly data = toSignal(
    this.list.hookToQuery(query => {
      const filters = (this.list.filter as Partial<CommentGetListInput>) || {};
      const input: CommentGetListInput = {
        repliedCommentId: this.commentId() ?? undefined,
        ...query,
        ...filters,
      };
      return this.commentService.getList(input);
    }),
    {
      initialValue: { items: [], totalCount: 0 } as PagedResultDto<CommentWithAuthorDto>,
    },
  );

  filterForm!: FormGroup;
  commentApproveStateOptions = commentApproveStateOptions;
  requireApprovement: boolean;

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.commentId.set(id);
        this.loadComment(id);
        this.createFilterForm();
        this.list.get();
      }
    });
    this.requireApprovement = this.commentEntityService.requireApprovement;
  }

  private createFilterForm() {
    this.filterForm = this.fb.group({
      creationStartDate: [null],
      creationEndDate: [null],
      author: [''],
      commentApproveState: [CommentApproveState.All],
    });
  }

  private loadComment(id: string) {
    this.commentService.get(id).subscribe(comment => {
      this.comment.set(comment);
    });
  }

  onFilter() {
    const formValue = this.filterForm.value;
    const filters: Partial<CommentGetListInput> = {
      author: formValue.author || undefined,
      commentApproveState: formValue.commentApproveState,
      repliedCommentId: this.commentId() ?? undefined,
      creationStartDate: formValue.creationStartDate || undefined,
      creationEndDate: formValue.creationEndDate || undefined,
    };

    this.list.filter = filters as any;
    this.list.get();
  }

  navigateToReply(id: string) {
    this.router.navigate(['/cms/comments', id]);
  }
}
