import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  Injector,
  OnInit,
  signal,
} from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { forkJoin, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { LocalizationPipe, RestService } from '@abp/ng.core';
import {
  ExtensibleFormComponent,
  FormPropData,
  generateFormFromProps,
  EXTENSIONS_IDENTIFIER,
} from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { ButtonComponent } from '@abp/ng.theme.shared';
import { ToastuiEditorComponent, prepareSlugFromControl } from '@abp/ng.cms-kit';
import {
  BlogPostAdminService,
  BlogPostDto,
  MediaDescriptorAdminService,
  EntityTagAdminService,
  CreateMediaInputWithStream,
  TagDto,
  BlogFeatureDto,
} from '@abp/ng.cms-kit/proxy';
import { eCmsKitAdminComponents } from '../../../enums';
import { BlogPostFormService } from '../../../services';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-blog-post-form',
  templateUrl: './blog-post-form.component.html',
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.BlogPostForm,
    },
  ],
  imports: [
    ButtonComponent,
    ExtensibleFormComponent,
    PageComponent,
    ToastuiEditorComponent,
    LocalizationPipe,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    NgxValidateCoreModule,
  ],
})
export class BlogPostFormComponent implements OnInit {
  private blogPostService = inject(BlogPostAdminService);
  private mediaService = inject(MediaDescriptorAdminService);
  private entityTagService = inject(EntityTagAdminService);
  private restService = inject(RestService);
  private injector = inject(Injector);
  private blogPostFormService = inject(BlogPostFormService);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  form: FormGroup;
  readonly blogPost = signal<BlogPostDto | null>(null);
  blogPostId: string | null = null;
  readonly isEditMode = signal(false);
  coverImageFile: File | null = null;
  readonly coverImagePreview = signal<string | null>(null);
  readonly tags = signal('');
  readonly isTagsEnabled = signal(true);

  readonly BLOG_POST_ENTITY_TYPE = 'BlogPost';

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEditMode.set(true);
      this.blogPostId = id;
      this.loadBlogPost(id);
    } else {
      this.isEditMode.set(false);
      this.buildForm();
    }
  }

  private loadBlogPost(id: string) {
    this.blogPostService.get(id).subscribe(blogPost => {
      this.blogPost.set(blogPost);
      if (blogPost.coverImageMediaId) {
        this.coverImagePreview.set(`/api/cms-kit/media/${blogPost.coverImageMediaId}`);
      }
      this.buildForm();
      this.loadTags(id);
    });
  }

  private loadTags(blogPostId: string) {
    this.restService
      .request<void, TagDto[]>({
        method: 'GET',
        url: `/api/cms-kit-public/tags/${this.BLOG_POST_ENTITY_TYPE}/${blogPostId}`,
      })
      .subscribe(tags => {
        if (tags && tags.length > 0) {
          this.tags.set(tags.map(t => t.name || '').join(', '));
        }
      });
  }

  private buildForm() {
    const currentBlogPost = this.blogPost();
    const data = new FormPropData(this.injector, currentBlogPost || {});
    const baseForm = generateFormFromProps(data);
    this.form = new FormGroup({
      ...baseForm.controls,
      content: new FormControl(currentBlogPost?.content || ''),
      coverImageMediaId: new FormControl(currentBlogPost?.coverImageMediaId || null),
    });
    prepareSlugFromControl(this.form, 'title', 'slug', this.destroyRef);

    const blogId = this.form.get('blogId')?.value || currentBlogPost?.blogId;
    if (blogId) {
      this.checkTagsFeature(blogId);
    }

    this.form
      .get('blogId')
      ?.valueChanges.pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(blogId => {
        if (blogId) {
          this.checkTagsFeature(blogId);
        }
      });
  }

  private checkTagsFeature(blogId: string) {
    this.restService
      .request<void, BlogFeatureDto>({
        method: 'GET',
        url: `/api/cms-kit/blogs/${blogId}/features/CmsKit.Tags`,
      })
      .subscribe(feature => {
        const { isEnabled } = feature || {};
        this.isTagsEnabled.set(isEnabled);
      });
  }

  onCoverImageChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.coverImageFile = input.files[0];
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.coverImagePreview.set(e.target.result);
      };
      reader.readAsDataURL(this.coverImageFile);
    }
  }

  removeCoverImage() {
    this.coverImageFile = null;
    this.coverImagePreview.set(null);
    this.form.patchValue({ coverImageMediaId: null });
  }

  private uploadCoverImage() {
    if (!this.coverImageFile) {
      return of(this.form.value.coverImageMediaId || null);
    }

    const input: CreateMediaInputWithStream = {
      name: this.coverImageFile.name,
      file: this.coverImageFile as any,
    };

    return this.mediaService.create('blogpost', input).pipe(
      tap(result => {
        this.form.patchValue({ coverImageMediaId: result.id });
      }),
      switchMap(result => of(result.id || null)),
    );
  }

  private setTags(blogPostId: string) {
    const tagsValue = this.tags();
    if (!tagsValue || !tagsValue.trim()) {
      return of(null);
    }

    const tagArray = tagsValue
      .split(',')
      .map(t => t.trim())
      .filter(t => t.length > 0);

    if (tagArray.length === 0) {
      return of(null);
    }

    return this.entityTagService.setEntityTags({
      entityType: this.BLOG_POST_ENTITY_TYPE,
      entityId: blogPostId,
      tags: tagArray,
    });
  }

  private executeSaveOperation(operation: 'save' | 'draft' | 'publish' | 'sendToReview') {
    this.uploadCoverImage()
      .pipe(
        tap(coverImageMediaId => {
          if (coverImageMediaId) {
            this.form.patchValue({ coverImageMediaId });
          }
        }),
        switchMap(() => {
          if (this.isEditMode()) {
            const currentBlogPost = this.blogPost();
            if (!currentBlogPost || !this.blogPostId) {
              return of(null);
            }
            return this.blogPostFormService.update(this.blogPostId, this.form, currentBlogPost);
          }

          switch (operation) {
            case 'save':
            case 'draft':
              return this.blogPostFormService.createAsDraft(this.form);
            case 'publish':
              return this.blogPostFormService.createAndPublish(this.form);
            case 'sendToReview':
              return this.blogPostFormService.createAndSendToReview(this.form);
            default:
              return of(null);
          }
        }),
        switchMap(result => {
          if (!result || !result.id) {
            return of(null);
          }
          return forkJoin([of(result), this.setTags(result.id)]);
        }),
      )
      .subscribe();
  }

  saveAsDraft() {
    this.executeSaveOperation('draft');
  }

  publish() {
    this.executeSaveOperation('publish');
  }

  sendToReview() {
    this.executeSaveOperation('sendToReview');
  }
}
