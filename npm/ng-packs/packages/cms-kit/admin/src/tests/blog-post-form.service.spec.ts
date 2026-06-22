/* eslint-disable */
import { describe, it, expect, beforeEach, jest } from '@jest/globals';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
// @ts-ignore - test types are resolved only in the library build context
import { ToasterService } from '@abp/ng.theme.shared';
// @ts-ignore - test types are resolved only in the library build context
import { BlogPostAdminService } from '@abp/ng.cms-kit/proxy';
import { BlogPostFormService } from '../services';

describe('BlogPostFormService', () => {
  let service: BlogPostFormService;
  let blogPostAdminService: any;
  let toasterService: any;
  let router: any;

  beforeEach(() => {
    blogPostAdminService = {
      create: jest.fn().mockReturnValue(of({})),
      createAndPublish: jest.fn().mockReturnValue(of({})),
      createAndSendToReview: jest.fn().mockReturnValue(of({})),
      update: jest.fn().mockReturnValue(of({})),
    };

    toasterService = {
      success: jest.fn(),
    };

    router = {
      navigate: jest.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        BlogPostFormService,
        { provide: BlogPostAdminService, useValue: blogPostAdminService },
        { provide: ToasterService, useValue: toasterService },
        { provide: Router, useValue: router },
      ],
    });

    service = TestBed.inject(BlogPostFormService);
  });

  function createValidForm(): FormGroup {
    // We don't rely on any specific controls, only on form.value and validity.
    return new FormGroup({});
  }

  function createInvalidForm(): FormGroup {
    const form = new FormGroup({});
    form.setErrors({ invalid: true });
    return form;
  }

  it('should throw when creating with invalid form', () => {
    const form = createInvalidForm();

    expect(() => service.create(form)).toThrowError('Form is invalid');
  });

  it('should call BlogPostAdminService.create and navigate on create', done => {
    const form = createValidForm();

    service.create(form).subscribe({
      next: () => {
        expect(blogPostAdminService.create).toHaveBeenCalledWith(form.value);
        expect(toasterService.success).toHaveBeenCalledWith('AbpUi::SavedSuccessfully');
        expect(router.navigate).toHaveBeenCalledWith(['/cms/blog-posts']);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should call BlogPostAdminService.create on createAsDraft', done => {
    const form = createValidForm();

    service.createAsDraft(form).subscribe({
      next: () => {
        expect(blogPostAdminService.create).toHaveBeenCalledWith(form.value);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should call BlogPostAdminService.createAndPublish on createAndPublish', done => {
    const form = createValidForm();

    service.createAndPublish(form).subscribe({
      next: () => {
        expect(blogPostAdminService.createAndPublish).toHaveBeenCalledWith(form.value);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should call BlogPostAdminService.createAndSendToReview on createAndSendToReview', done => {
    const form = createValidForm();

    service.createAndSendToReview(form).subscribe({
      next: () => {
        expect(blogPostAdminService.createAndSendToReview).toHaveBeenCalledWith(form.value);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should throw when updating with invalid form or missing blog post', () => {
    const form = createInvalidForm();

    expect(() => service.update('id', form, {} as any)).toThrowError(
      'Form is invalid or blog post is missing',
    );

    const validForm = createValidForm();
    expect(() => service.update('id', validForm, null as any)).toThrowError(
      'Form is invalid or blog post is missing',
    );
  });

  it('should call BlogPostAdminService.update and navigate on update', done => {
    const form = createValidForm();
    const blogPost = { id: '1', title: 't' };

    service.update('1', form, blogPost).subscribe({
      next: () => {
        expect(blogPostAdminService.update).toHaveBeenCalled();
        expect(toasterService.success).toHaveBeenCalledWith('AbpUi::SavedSuccessfully');
        expect(router.navigate).toHaveBeenCalledWith(['/cms/blog-posts']);
        done();
      },
      error: err => done(err as any),
    });
  });
});
