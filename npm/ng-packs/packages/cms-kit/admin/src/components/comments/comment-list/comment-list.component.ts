import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgbDateAdapter, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { ListService, PagedResultDto, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleModule, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { PageModule } from '@abp/ng.components/page';
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
  selector: 'abp-comment-list',
  templateUrl: './comment-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.CommentList,
    },
  ],
  viewProviders: [
    {
      provide: NgbDateAdapter,
      useClass: DateTimeAdapter,
    },
  ],
  imports: [
    ExtensibleModule,
    PageModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    CommonModule,
    LocalizationPipe,
    FormInputComponent,
    ButtonComponent,
  ],
})
export class CommentListComponent implements OnInit {
  data: PagedResultDto<CommentWithAuthorDto> = { items: [], totalCount: 0 };

  readonly list = inject(ListService<CommentGetListInput>);
  readonly commentEntityService = inject(CommentEntityService);

  private commentService = inject(CommentAdminService);
  private fb = inject(FormBuilder);

  filterForm!: FormGroup;
  commentApproveStateOptions = commentApproveStateOptions;
  requireApprovement: boolean;

  ngOnInit() {
    this.createFilterForm();
    this.hookToQuery();
    this.requireApprovement = this.commentEntityService.requireApprovement;
  }

  private createFilterForm() {
    this.filterForm = this.fb.group({
      creationStartDate: [null],
      creationEndDate: [null],
      author: [''],
      entityType: [''],
      commentApproveState: [CommentApproveState.All],
    });
  }

  onFilter() {
    const formValue = this.filterForm.value;
    const filters: Partial<CommentGetListInput> = {
      author: formValue.author || undefined,
      entityType: formValue.entityType || undefined,
      commentApproveState: formValue.commentApproveState,
      creationStartDate: formValue.creationStartDate || undefined,
      creationEndDate: formValue.creationEndDate || undefined,
    };

    this.list.filter = filters as any;
    this.list.get();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => {
        const filters = (this.list.filter as Partial<CommentGetListInput>) || {};
        const input: CommentGetListInput = {
          ...query,
          ...filters,
        };
        return this.commentService.getList(input);
      })
      .subscribe(res => (this.data = res));
  }
}
