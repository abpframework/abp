import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';
import {
  BlogPostAdminService,
  CreateBlogPostDto,
  UpdateBlogPostDto,
  BlogPostDto,
} from '@abp/ng.cms-kit/proxy';

@Injectable({
  providedIn: 'root',
})
export class BlogPostFormService {
  private blogPostService = inject(BlogPostAdminService);
  private toasterService = inject(ToasterService);
  private router = inject(Router);

  create(form: FormGroup): Observable<BlogPostDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    return this.blogPostService.create(form.value as CreateBlogPostDto).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/blog-posts']);
      }),
    );
  }

  createAsDraft(form: FormGroup): Observable<BlogPostDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    return this.blogPostService.create(form.value as CreateBlogPostDto).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/blog-posts']);
      }),
    );
  }

  createAndPublish(form: FormGroup): Observable<BlogPostDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    return this.blogPostService.createAndPublish(form.value as CreateBlogPostDto).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/blog-posts']);
      }),
    );
  }

  createAndSendToReview(form: FormGroup): Observable<BlogPostDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    return this.blogPostService.createAndSendToReview(form.value as CreateBlogPostDto).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/blog-posts']);
      }),
    );
  }

  update(blogPostId: string, form: FormGroup, blogPost: BlogPostDto): Observable<BlogPostDto> {
    if (!form.valid || !blogPost) {
      throw new Error('Form is invalid or blog post is missing');
    }

    const formValue = {
      ...blogPost,
      ...form.value,
    } as UpdateBlogPostDto;

    return this.blogPostService.update(blogPostId, formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/blog-posts']);
      }),
    );
  }
}
